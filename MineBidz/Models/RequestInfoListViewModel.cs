using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MineBidz.Models
{
    public class RequestInfoListViewModel
    {
        public int RequestInfoId { get; set; }
        public string Condition {get; set;}
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public string Location { get; set; }
        public string Country { get; set; }
        public string RefNumber { get; set; }
        public string BidEnd { get; set; }
        public string FormName { get; set; }
        public string FormTitle { get; set; }
        public string BidName { get; set; }
        public string BidStart { get; set; }
        public string DocumentInfo { get; set; }
        public bool Approved { get; set; }
    }
}