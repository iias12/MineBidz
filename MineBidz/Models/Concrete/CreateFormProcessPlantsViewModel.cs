﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;
using Domain.Concrete;

namespace MineBidz.Models
{
    public class CreateFormProcessPlantsViewModel : CreateFormViewModel
    {
        public ProcessPlants DetailsInfo { get; set; }
    }
}