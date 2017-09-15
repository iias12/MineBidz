using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;
using Domain.Concrete;

namespace MineBidz.Models.Concrete
{
    public class DisplayOtherConsumablesViewModel : DisplayBaseViewModel
    {
        public OtherConsumables Details { get; set; }
    }
}