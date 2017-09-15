using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class Excavator
    {
        public string Quantity { get; set; }
        public string ExcavatorType { get; set; }
        public string ExcavatorTypeOther { get; set; }
        public string SizeClassMetricTon { get; set; }
        public string SizeClassMetricTonOther { get; set; }
        public string BucketCapacityYd3 { get; set; }
        public string RecommendedPower { get; set; }
        public string RecommendedPowerUnit { get; set; }
        public string RecommendedPowerUnitOther { get; set; }
        public string HoursNoGreaterThanHrs { get; set; }
    }
}
