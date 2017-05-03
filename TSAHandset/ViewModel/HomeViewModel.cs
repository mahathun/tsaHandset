using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSAHandset.Models;

namespace TSAHandset.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        public List<Request> AllRequests { get; set; }
        public List<Request> PendingRequests { get; set; }
        public List<Request> AcceptedRequests { get; set; }
        public List<Request> RejectedRequests { get; set; }
        public List<Request> UserRequests { get; set; }

    }
}