using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MineBidz.Models
{
    public class RequestInfoFilteredListViewModel
    {
        public List<RequestInfoListViewModel> RequestInfoList { get; set; }
        public RequestInfoFilters Filters { get; set; }
    }
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
        public string ClassName { get; set; }
        public string FormTitle { get; set; }
        public string BidName { get; set; }
        public string BidStart { get; set; }
        public string DocumentInfo { get; set; }
        public bool Approved { get; set; }
        public bool VendorCanContact { get; set; }
    }

    public class RequestInfoFilters
    {
        public int CategoryId { get; set; }
        public System.Web.Mvc.SelectList Categories { get; set; }
        public string CountryCode { get; set; }
        public System.Web.Mvc.SelectList Countries { get; set; }
        public string StateProvinceCode { get; set; }
        public System.Web.Mvc.SelectList StatesProvinces { get; set; }
        public string Title { get; set; }
    }
}