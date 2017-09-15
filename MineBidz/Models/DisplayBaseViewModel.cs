using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MineBidz.Models
{
    public class DisplayBaseViewModel: CreateFormViewModel
    {
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public int FormInfoId { get; set; }
        public string Contry { get; set; }
        public string Location { get; set; }
        public bool UserIsAuthenicated { get; set; }
        public bool UserCanBid { get; set; }
        public string DocumentInfo { get; set; }
        public string DetailsInfoJson { get; set; }
    }
}