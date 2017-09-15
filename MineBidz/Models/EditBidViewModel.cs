using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MineBidz.Models
{
    public class EditBidViewModel : BaseBidViewModel
    {
        public int Id { get; set; }
        public string EngineeringDesign { get; set; }
        public bool Approved { get; set; }
    }
}