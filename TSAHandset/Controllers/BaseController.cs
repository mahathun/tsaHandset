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
    public class BaseController : Controller
    {
        protected ApplicationDbContext _context;


        protected string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        protected string appKey = ConfigurationManager.AppSettings["ida:ClientSecret"];
        protected string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        protected string graphResourceID = "https://graph.windows.net";
        protected string teamISGGroupId = "7a20c5a2-b1e9-4206-92be-3b421fc9f957";//security group id of the team iSG group



        protected BaseController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        //GET the logged in User with MemberOf expanded
        protected async Task<IUser> GetLoggedInUser()
        {

            string userObjectID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            ActiveDirectoryClient activeDirectoryClient = await GetActivieDirectoryClient();

            // use the token for querying the graph to get the user details

            var result = await activeDirectoryClient.Users
                .Where(u => u.ObjectId.Equals(userObjectID))
                .Expand(m => m.MemberOf)
                .ExecuteAsync();
            IUser user = result.CurrentPage.ToList().First();

            return user;


        }

        //Get the activedirectory client token
        protected async Task<ActiveDirectoryClient> GetActivieDirectoryClient()
        {

            string tenantID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            string userObjectID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            Uri servicePointUri = new Uri(graphResourceID);
            Uri serviceRoot = new Uri(servicePointUri, tenantID);
            ActiveDirectoryClient activeDirectoryClient = new ActiveDirectoryClient(serviceRoot,
                  async () => await GetTokenForApplication());

            return activeDirectoryClient;
        }

        //GET the Group
        protected async Task<IGroup> GetGroup(string groupObjectId)
        {

            ActiveDirectoryClient activeDirectoryClient = await GetActivieDirectoryClient();
            var groupResult = await activeDirectoryClient.Groups.Expand(g => g.Owners).Where(g => g.ObjectId == groupObjectId).ExecuteAsync();



            return groupResult.CurrentPage.ToList().First();


        }

        //GET all the Groups with expanded Owner peroperty
        protected async Task<List<IGroup>> GetGroupsWithOwner()
        {

            ActiveDirectoryClient activeDirectoryClient = await GetActivieDirectoryClient();
            var groupResult = await activeDirectoryClient.Groups.Expand(g => g.Owners).ExecuteAsync();
            
            return groupResult.CurrentPage.ToList();

        }

        //Get the token for the application in order to communicate with AD
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

        //GET Avarage duration in days to complete a request
        public int GetAverageDurationInDays()
        {
            int days = 0;
            TimeSpan totalDuration = new TimeSpan();
            var requests = _context.Requests.Include("Progress").Where(r => r.ProgressId == 4).ToList();

            foreach(var request in requests)
            {
                totalDuration += (DateTime)request.CompletedDate - request.RequestDate;
            }

            return (totalDuration ==null || totalDuration.Days == 0) ? 0 : totalDuration.Days / requests.Count;
            
        }

        //Check if the logged in user is a member of the team iSG
        protected async Task<bool> isAnISGMember()
        {
            bool isISGMember = false;
            //current logged in user
            IUser user = await GetLoggedInUser();


            foreach (var securityGroupUserMemberOf in user.MemberOf.CurrentPage.ToList())
            {
                if (securityGroupUserMemberOf.ObjectId == teamISGGroupId)
                {
                    isISGMember = true;
                }
            }

            return isISGMember;
        }
    }
}