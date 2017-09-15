using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;
using Domain.Concrete;

namespace MineBidz.Models
{
    public class CreateFormVehicleViewModel : CreateFormViewModel
    {
        public Vehicle DetailsInfo { get; set; }
    }
}