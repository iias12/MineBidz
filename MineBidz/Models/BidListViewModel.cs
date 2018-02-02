using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MineBidz.Models
{
    public class BidListViewModel
    {
        public int Id { get; set; }
        public int RequestInfoId { get; set; }
        public string RefNumber { get; set; }
        public string RefNumberRequest { get; set; }
        public string DocumentInfo { get; set; }
        public string RequestDocumentInfo { get; set; }
        public string Description { get; set; }
        public bool Approved { get; set; }
        public bool Accepted { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public PaymentViewModel Payment { get; set; }
    }
}