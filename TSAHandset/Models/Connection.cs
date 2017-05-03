using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSAHandset.Models
{
    public class Connection
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Plan Plan { get; set; }
        public int? PlanId { get; set; }
        public Handset Handset { get; set; }
        public int? HandsetId { get; set; }
    }
}