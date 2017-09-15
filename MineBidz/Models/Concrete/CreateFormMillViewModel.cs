using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;
using Domain.Concrete;

namespace MineBidz.Models
{
    public class CreateFormMillViewModel : CreateFormViewModel
    {
        public Mill DetailsInfo { get; set; }
    }
}