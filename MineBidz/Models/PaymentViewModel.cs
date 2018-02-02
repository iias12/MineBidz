using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Domain;

namespace MineBidz.Models
{
    public class PaymentViewModel
    {
        public Guid PaymentId {get; set;}
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Credit Card Number is required")]
        public string CreditCardNumber { get; set; }
        [Required(ErrorMessage = "Name on card is required")]
        public string NameOnCard { get; set; }
        [Required(ErrorMessage = "Expiration month is required")]
        public int ExpirationMonth { get; set; }
        [Required(ErrorMessage = "Expiration year is required")]
        public int ExpirationYear { get; set; }
        [Required(ErrorMessage = "Security code is required")]
        public string SecurityCode { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string StreetAddress { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "Province/State is required")]
        public string ProvinceStateCode { get; set; }
        [Required(ErrorMessage = "Country is required")]
        public string CountryCode { get; set; }
        [Required(ErrorMessage = "Postal Code is required")]
        public string PostalCode { get; set; }
        public bool Processed { get; set; }
    }
}