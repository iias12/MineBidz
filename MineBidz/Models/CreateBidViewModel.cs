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
        public bool Acknowledged { get; set; }
    }
}