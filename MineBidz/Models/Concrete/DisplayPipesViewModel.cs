using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;
using Domain.Concrete;

namespace MineBidz.Models.Concrete
{
    public class DisplayPipeViewModel : DisplayBaseViewModel
    {
        public Pipe Details { get; set; }
    }
}