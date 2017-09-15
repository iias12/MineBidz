using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Concrete.Common;

namespace Domain.Concrete
{
    public class Dewatering
    {
        public Product Product { get; set; }

        public string DewateringType { get; set; }
        public string DewateringTypeOther { get; set; }
        public string CompactedBulkDensityofDrySolidsSG { get; set; }
        public string SpecificGravityOfLiquidTM3 { get; set; }
        public string PercentDrySolidByWeightInSlurry { get; set; }
        public string TotalWeightOfDrySolidsPerDay { get; set; }
        public string TotalWeightOfDrySolidsPerDayUnit { get; set; }
        public string PercentDrySolidByWeightInCake { get; set; }
        public string TotalCycleTimeIfPressureFilterMin { get; set; }
        public string DailyAvailabilityPercent24HrDay { get; set; }
        public string NumberOfUnitsRequiredEa { get; set; }
        public string Feed80PercentPassingExpectedMM { get; set; }
        public bool MotorIncluded { get; set; }

        public Motor Motor { get; set; }
    }
}
