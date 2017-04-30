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
    public class RequestController : Controller
    {

        private ApplicationDbContext _context;
        private string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private string appKey = ConfigurationManager.AppSettings["ida:ClientSecret"];
        private string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private string graphResourceID = "https://graph.windows.net";

        public RequestController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Request
        public ActionResult Index()
        {

            return HttpNotFound();
        }


        //NEW REQUESTS
        public ActionResult New()
        {

            var requestViewModel = new RequestFormViewModel()
            {
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
                string tenantID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
                string userObjectID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                try
                {
                    Uri servicePointUri = new Uri(graphResourceID);
                    Uri serviceRoot = new Uri(servicePointUri, tenantID);
                    ActiveDirectoryClient activeDirectoryClient = new ActiveDirectoryClient(serviceRoot,
                          async () => await GetTokenForApplication());

                    // use the token for querying the graph to get the user details

                    var result = await activeDirectoryClient.Users
                        .Where(u => u.ObjectId.Equals(userObjectID))
                        .Expand(m=>m.MemberOf)
                        .ExecuteAsync();
                    IUser user = result.CurrentPage.ToList().First();

                    var newRequest = new Request();
                    //if a valid request
                    switch (request.RequestTypeId)
                    {
                        case 1://connection only
                            
                            newRequest.RequestUserId = user.ObjectId;
                            newRequest.SecurityGroupId = user.MemberOf.CurrentPage.First().ObjectId;
                            newRequest.PlanId = request.PlanId;
                            newRequest.RequestDate = DateTime.Now;
                            newRequest.ProgressId = 1;
                            newRequest.RequestTypeId = request.RequestTypeId;                     

                            break;
                        case 2://handset only

                            newRequest.RequestUserId = user.ObjectId;
                            newRequest.SecurityGroupId = user.MemberOf.CurrentPage.First().ObjectId;
                            newRequest.HandsetId = request.HandsetId;
                            newRequest.RequestDate = DateTime.Now;
                            newRequest.ProgressId = 1;
                            newRequest.RequestTypeId = request.RequestTypeId;
                            
                            break;
                        case 3://both connection and handset

                            newRequest.RequestUserId = user.ObjectId;
                            newRequest.SecurityGroupId = user.MemberOf.CurrentPage.First().ObjectId;
                            newRequest.PlanId = request.PlanId;
                            newRequest.HandsetId = request.HandsetId;
                            newRequest.RequestDate = DateTime.Now;
                            newRequest.ProgressId = 1;
                            newRequest.RequestTypeId = request.RequestTypeId;

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

        public async Task<string> GetTokenForApplication()
        {
            string signedInUserID = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
            string tenantID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            string userObjectID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            // get a token for the Graph without triggering any user interaction (from the cache, via multi-resource refresh token, etc)
            ClientCredential clientcred = new ClientCredential(clientId, appKey);
            // initialize AuthenticationContext with the token cache of the currently signed in user, as kept in the app's database
            AuthenticationContext authenticationContext = new AuthenticationContext(aadInstance + tenantID, new ADALTokenCache(signedInUserID));
            AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenSilentAsync(graphResourceID, clientcred, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));
            return authenticationResult.AccessToken;
        }
    }
    }