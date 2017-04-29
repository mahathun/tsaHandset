using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSAHandset.Models
{
    public class Plan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short MonthlyCharge { get; set; }
        public byte MonthlyDataAllowanceInGB { get; set; }
        public short MonthlyMinuteAllowance { get; set; }
        public short MonthlyTextAllowance { get; set; }
        public short AccountCalling { get; set; }
    }
}