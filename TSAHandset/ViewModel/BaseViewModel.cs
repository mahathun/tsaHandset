using Microsoft.Azure.ActiveDirectory.GraphClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSAHandset.ViewModel
{
    public class BaseViewModel
    {
        public IUser User { get; set; }
    }
}