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
    public class HomeController : BaseController
    {
        private ApplicationDbContext _context ;
        

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        //DISPLAYING MAIN LANDING PAGE
        public async Task<ActionResult> Index()
        {
            try
            {
                //get all the security groups(Departments)
                var groupResult = await GetGroupsWithOwner();

                //current logged in user
                IUser user = await GetLoggedInUser();
                List<IGroup> groups = groupResult;

                bool isAnAdmin = false;
                bool isISGMember = await isAnISGMember();

                List<IGroup> listOfGroupsAdminOf = new List<IGroup>();
      

                foreach(var group in groups)
                {
                    foreach(var owner in group.Owners.CurrentPage)
                    {
                        if(owner.ObjectId == user.ObjectId)
                        {
                            isAnAdmin = true;
                            listOfGroupsAdminOf.Add(group);
                        }
                    }

                }


                if (isAnAdmin)
                {
                    //if the logged user is an admin of a group/s
                    List<Request> allRequestsAssignedToTheAdmin = new List<Request>();
                    foreach(var group in listOfGroupsAdminOf)
                    {
                        var result = _context.Requests.Include("Progress").Where(r => r.SecurityGroupId == group.ObjectId).ToList();
                        allRequestsAssignedToTheAdmin.AddRange(result);
                    }

                    var homeViewModel = new HomeViewModel() {
                        
                        User = user,
                        AllRequests = allRequestsAssignedToTheAdmin,
                        PendingRequests = allRequestsAssignedToTheAdmin.Where(r => r.ProgressId == 1).ToList(),
                        AcceptedRequests = allRequestsAssignedToTheAdmin.Where(r => r.ProgressId == 2).ToList(),
                        RejectedRequests = allRequestsAssignedToTheAdmin.Where(r => r.ProgressId == 3).ToList(),
                        UserRequests = _context.Requests.Where(r=>r.RequestUserId == user.ObjectId).ToList(),
                        isISGMember = isISGMember,
                        RequestAssignedToiSG = _context.Requests.Include("Progress").ToList().Where(r => r.ProgressId == 2).ToList()
                    };
                    
                    return View("AdminIndex", homeViewModel);
                }
                else
                {
                    var requests = _context.Requests.Include("Progress").Where(r => r.RequestUserId == user.ObjectId).ToList();
                    var homeViewModel = new HomeViewModel()
                    {
                        User = user,
                        UserRequests = requests,
                        isISGMember =isISGMember,
                        RequestAssignedToiSG = _context.Requests.Include("Progress").ToList().Where(r=>r.ProgressId==2).ToList()
                    };

                    return View("Index",homeViewModel);
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
                return View("Relogin");
            }

            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}