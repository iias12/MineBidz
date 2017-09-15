using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class HaulTruck
    {
        public string Quantity { get; set; }
        public string ProductionTarget { get; set; }
        public string ProductionTargetUnit { get; set; }
        public string BodyType { get; set; }
        public string BodyTypeOther { get; set; }
        public string Types { get; set; }
        public string SizeClasses { get; set; }
        public string CapacityYd3 { get; set; }
        public string RecommendedPower { get; set; }
        public string RecommendedPowerUnit { get; set; }
        public string RecommendedPowerUnitOther { get; set; }
        public string HoursNoGreaterThanHrs { get; set; }
        public string Cab { get; set; }
    }
}
