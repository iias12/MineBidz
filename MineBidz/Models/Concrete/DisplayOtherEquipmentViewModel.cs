using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;
using Domain.Concrete;

namespace MineBidz.Models.Concrete
{
    public class DisplayOtherEquipmentViewModel : DisplayBaseViewModel
    {
        public OtherEquipment Details { get; set; }
    }
}