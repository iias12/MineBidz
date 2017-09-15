using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;
using Domain.Concrete;

namespace MineBidz.Models.Concrete
{
    public class DisplayTractorsViewModel : DisplayBaseViewModel
    {
        public Tractors Details { get; set; }
    }
}