using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;
using Domain.Concrete;

namespace MineBidz.Models
{
    public class CreateFormDewateringViewModel : CreateFormViewModel
    {
        public Dewatering DetailsInfo { get; set; }
    }
}