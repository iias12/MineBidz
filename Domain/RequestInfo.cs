using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class RequestInfo
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int RequestFormId { get; set; }
        public string ClassName { get; set; }
        public int CategoryId { get; set; }
        public int SubcategoryId { get; set; }
        public ContactInfo CompanyInfo { get; set; }
        public ContactInfo DeliveryInfo { get; set; }
        public BidInfo BidInfo { get; set; }
        public string DetailsInfoJson { get; set; }
        public bool Approved { get; set; }
        public List<Condition> ConditionList { get; set; }
        public string Description { get; set; }
        public string DocumentInfo { get; set; }
    }
}
