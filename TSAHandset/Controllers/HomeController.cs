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
    public class HomeController : Controller
    {
        private ApplicationDbContext _context ;
        private string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private string appKey = ConfigurationManager.AppSettings["ida:ClientSecret"];
        private string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private string graphResourceID = "https://graph.windows.net";

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

            string tenantID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            string userObjectID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            try
            {
                Uri servicePointUri = new Uri(graphResourceID);
                Uri serviceRoot = new Uri(servicePointUri, tenantID);
                ActiveDirectoryClient activeDirectoryClient = new ActiveDirectoryClient(serviceRoot,
                      async () => await GetTokenForApplication());

                ActiveDirectoryClient activeDirectoryClient1 = new ActiveDirectoryClient(serviceRoot,
                      async () => await GetTokenForApplication());

                // use the token for querying the graph to get the user details

                var userResult = await activeDirectoryClient.Users
                    .Where(u => u.ObjectId.Equals(userObjectID))
                    .ExecuteAsync();

                var groupResult = await activeDirectoryClient1.Groups.Expand(u=>u.Owners).ExecuteAsync();

                //current logged in user
                IUser user = userResult.CurrentPage.ToList().First();
                List<IGroup> groups = groupResult.CurrentPage.ToList();

                bool isAnAdmin = false;
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
                        UserRequests = _context.Requests.Where(r=>r.RequestUserId == user.ObjectId).ToList()
                    };
                    
                    return View("AdminIndex", homeViewModel);
                }
                else
                {
                    var requests = _context.Requests.Include("Progress").Where(r => r.RequestUserId == user.ObjectId).ToList();
                    var homeViewModel = new HomeViewModel()
                    {
                        User = user,
                        UserRequests = requests
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