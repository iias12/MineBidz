using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Bid
    {
        public int Id { get; set; }
        public ContactInfo CompanyInfo { get; set; }
        public int RequestInfoId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string ReferenceNumber { get; set; }
        public int UserId { get; set; }
        public string EngineeringDesign { get; set; }
        public bool Approved { get; set; }
        public bool Accepted { get; set; }
        public CcPayment CcPayment { get; set; }
    }

    public class CcPayment
    {
        public Guid PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string CreditCardNumber { get; set; }
        public string NameOnCard { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }
        public string SecurityCode { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string ProvinceStateCode { get; set; }
        public string CountryCode { get; set; }
        public string PostalCode { get; set; }
    }

}
