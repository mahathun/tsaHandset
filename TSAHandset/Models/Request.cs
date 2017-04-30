using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TSAHandset.Models
{
    public class Request
    {
        public int Id { get; set; }
        public string RequestUserId { get; set; }
        public string SecurityGroupId { get; set; }
        public Plan Plan { get; set; }
        public int? PlanId { get; set; }
        public Handset Handset { get; set; }
        public int? HandsetId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int Notification { get; set; }
        public ProgressLevel Progress { get; set; }
        public byte ProgressId { get; set; }
        public RequestType RequestType { get; set; }
        [Required]
        [Range(1,3,ErrorMessage ="Request Type is Required")]
        public byte RequestTypeId { get; set; }

    }
}