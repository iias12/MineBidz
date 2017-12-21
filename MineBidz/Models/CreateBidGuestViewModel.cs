using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Domain;

namespace MineBidz.Models
{
    public class CreateBidGuestViewModel : CreateBidViewModel
    {
        public PaymentViewModel Payment { get; set; }
    }
}