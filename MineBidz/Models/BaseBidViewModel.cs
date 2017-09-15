using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Domain;

namespace MineBidz.Models
{
    public class BaseBidViewModel
    {
        public ContactInfo CompanyInfo { get; set; }
        public RequestInfoListViewModel RequestInfo { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Reference Number is required")]
        public string ReferenceNumber { get; set; }
    }
}