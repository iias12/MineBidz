using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;

namespace MineBidz.Models
{
    public class CreateFormViewModel
    {
        public ContactInfo CompanyInfo { get; set; }
        public ContactInfo DeliveryInfo { get; set; }
        public BidInfo BidInfo { get; set; }
        public string Description { get; set; }
        public bool New { get; set; }
        public bool Used { get; set; }
        public bool Rental { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public Repository Repository { get; set; }
        public HttpPostedFileBase EngineeringDesign { get; set; }
        public string ClassName {get; set;}
        public string FormName { get; set; }
        public string FormTitle { get; set; }
        public bool Acknowledged { get; set; }
        public string DetailsInfoJson { get; set; }
        public int RequestInfoId { get; set; }
    }
}