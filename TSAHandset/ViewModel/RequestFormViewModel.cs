using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSAHandset.Models;

namespace TSAHandset.ViewModel
{
    public class RequestFormViewModel :BaseViewModel
    {
        public List<Plan> Plans { get; set; }
        public List<Handset> Handsets { get; set; }
        public List<RequestType> RequestTypes { get; set; }

        public Request Request { get; set; }
    }
}