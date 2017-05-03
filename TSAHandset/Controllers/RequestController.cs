using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TSAHandset.Models;
using TSAHandset.ViewModel;

namespace TSAHandset.Controllers
{
    [Authorize]
    public class RequestController : BaseController
    {

        private ApplicationDbContext _context;


        //constructor
        public RequestController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // Landing page Request
        public ActionResult Index()
        {
            return HttpNotFound();
        }


        //NEW REQUESTS
        public async Task<ActionResult> New()
        {
            

            var requestViewModel = new RequestFormViewModel()
            {
                User = await GetLoggedInUser(),   
                Request = new Request(),
                Plans = _context.Plans.ToList(),
                Handsets = _context.Handsets.ToList(),
                RequestTypes = _context.RequestTypes.ToList()
            };


            return View(requestViewModel);
        }

        //SAVING NEW REQUESTS
        public async Task<ActionResult> Submit(Request request)
        {
            if (!ModelState.IsValid)
            {
                var requestViewModel = new RequestFormViewModel()
                {
                    Request = request,
                    Handsets = _context.Handsets.ToList(),
                    Plans = _context.Plans.ToList(),
                    RequestTypes = _context.RequestTypes.ToList()
                };

                return View("New", requestViewModel);

            }
            else
            {
              
                try
                {
                    // use the token for querying the graph to get the user details
                    IUser user = await GetLoggedInUser();

                    var newRequest = new Request();
                    newRequest.RequestUserId = user.ObjectId;
                    newRequest.RequestUserName = (user.DisplayName.Length>0)? user.DisplayName: user.UserPrincipalName;
                    newRequest.RequestDate = DateTime.Now;
                    newRequest.ProgressId = 1;
                    newRequest.RequestTypeId = request.RequestTypeId;
                    //if a valid request
                    switch (request.RequestTypeId)
                    {
                        case 1://connection only
                           
                            newRequest.SecurityGroupId = user.MemberOf.CurrentPage.First().ObjectId;
                            newRequest.PlanId = request.PlanId;

                            break;
                        case 2://handset only
                            
                            newRequest.SecurityGroupId = user.MemberOf.CurrentPage.First().ObjectId;
                            newRequest.HandsetId = request.HandsetId;
                            
                            break;
                        case 3://both connection and handset
                            
                            newRequest.SecurityGroupId = user.MemberOf.CurrentPage.First().ObjectId;
                            newRequest.PlanId = request.PlanId;
                            newRequest.HandsetId = request.HandsetId;

                            break;
                        default:
                            break;
                    }

                    _context.Requests.Add(newRequest);
                    _context.SaveChanges();
                }
                catch (AdalException)
                {
                    // Return to error page.
                    
                    return View("Error");
                }
                // if the above failed, the user needs to explicitly re-authenticate for the app to obtain the required token
                catch (Exception)
                {
                    return RedirectToAction("Relogin", "UserProfile");
                    
                }
                return RedirectToAction("Index","Home");
            }

         
        }

        //Managing the aproval action on a request
        public async Task<ActionResult> AcceptRequest(int Id)
        {
            try
            {
                IUser currentUser = await GetLoggedInUser();
                var request = _context.Requests.Where(r=>r.Id==Id).ToList().First();

                var group = await GetGroup(request.SecurityGroupId);
                
                foreach(var owner in group.Owners.CurrentPage.ToList())
                {
                    if(owner.ObjectId == currentUser.ObjectId)
                    {
                        //logged in user has permission to authorize
                        request.ProgressId = 2; // request accepts
                        request.ApprovedDate = DateTime.Now;

                        _context.SaveChanges();
                    }
                   
                }
            }
            catch (AdalException)
            {
                // Return to error page.

                return View("Error");
            }
            // if the above failed, the user needs to explicitly re-authenticate for the app to obtain the required token
            catch (Exception)
            {
                return RedirectToAction("Relogin", "UserProfile");

            }

            return RedirectToAction("Index", "Home");
        }

        //Managing the reject action on a request
        public async Task<ActionResult> RejectRequest(int Id)
        {
            try
            {
                IUser currentUser = await GetLoggedInUser();
                var request = _context.Requests.Where(r => r.Id == Id).ToList().First();

                var group = await GetGroup(request.SecurityGroupId);
                
                foreach (var owner in group.Owners.CurrentPage.ToList())
                {
                    if (owner.ObjectId == currentUser.ObjectId)
                    {
                        //logged in user has permission to authorize
                        request.ProgressId = 3; // request reject
                        request.ApprovedDate = DateTime.Now;

                        _context.SaveChanges();
                    }

                }
            }
            catch (AdalException)
            {
                // Return to error page.

                return View("Error");
            }
            // if the above failed, the user needs to explicitly re-authenticate for the app to obtain the required token
            catch (Exception)
            {
                return RedirectToAction("Relogin", "UserProfile");

            }

            return RedirectToAction("Index", "Home");
        }
        
        //Managing the complete action on a request
        public async Task<ActionResult> CompleteRequest(int Id)
        {

            bool isISGMember = await isAnISGMember();

            //if the current logged in user is not a member of the team iSG
            if (!isISGMember)
            {
                return View("Unauthorized");
            }

            //var user = await GetLoggedInUser();
            var request = _context.Requests.Where(r => r.Id == Id).ToList().First();
            var existingConnections = _context.Connections.Where(c => c.UserId == request.RequestUserId).ToList();

            if(existingConnections.Count == 0)
            {
                //Create a new connection profile for the user as there are no existing connections

                var connection = new Connection()
                {
                    UserId = request.RequestUserId,
                    PlanId = request.PlanId,
                    HandsetId = request.HandsetId
                };

                _context.Connections.Add(connection);
            }
            else
            {
                //if there is an existing connection update it
                var connection = existingConnections.First();

                connection.PlanId = request.PlanId;
                connection.HandsetId = request.HandsetId;


            }

            request.ProgressId = 4;//completed
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        
       
    }
}