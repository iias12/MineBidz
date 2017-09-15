using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;

namespace MineBidz.Models
{
    public class RequestFormViewModel
    {
        public IEnumerable<RequestForm> RequestForms { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<Subcategory> SubcategoryList { get; set; }
    }
}