using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Sub Category is required")]
        public int SubCategoryId { get; set; }
        public Repository Repository { get; set; }
        public HttpPostedFileBase EngineeringDesign { get; set; }
        [Required(AllowEmptyStrings=false, ErrorMessage="Equipment name is required")]
        public string EquipmentId {get; set;}
        public string FormName { get; set; }
        public string FormTitle { get; set; }
        [Range(typeof(bool), "true", "true", ErrorMessage = "You have to accept")]
        public bool Acknowledged { get; set; }
        public bool VendorCanContact { get; set; }
        public string DetailsInfoJson { get; set; }
        public int RequestInfoId { get; set; }
        public System.Web.Mvc.SelectList Categories { get; set; }
        public System.Web.Mvc.SelectList Subcategories { get; set; }
        public System.Web.Mvc.SelectList RequestForms { get; set; }

    }
}