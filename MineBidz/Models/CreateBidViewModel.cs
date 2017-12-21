using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Domain;

namespace MineBidz.Models
{
    public class CreateBidViewModel : BaseBidViewModel
    {
        public HttpPostedFileBase EngineeringDesign { get; set; }
        [Range(typeof(bool), "true", "true", ErrorMessage = "You have to accept")]
        public bool Acknowledged { get; set; }
    }
}