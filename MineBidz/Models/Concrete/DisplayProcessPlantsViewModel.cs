﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;
using Domain.Concrete;

namespace MineBidz.Models.Concrete
{
    public class DisplayProcessPlantsViewModel : DisplayBaseViewModel
    {
        public ProcessPlants Details { get; set; }
    }
}