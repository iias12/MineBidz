using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class Loader
    {
        public int QuantityEach { get; set; }
        public int ProductionTargetNumber { get; set; }
        public string ProductionTargetUnit { get; set; }
        public string BodyType { get; set; }
        public string BodyTypeOther { get; set; }
        public string CabinType { get; set; }
        public string Steering { get; set; }
        public string SteeringOther { get; set; }
        public string SizeClassMetricTonPayload { get; set; }
        public string SizeClassMetricTonPayloadOther { get; set; }
        public int CapacityQubicMeters { get; set; }
        public string RecommendedPower { get; set; }
        public string RecommendedPowerUnit { get; set; }
        public string RecommendedPowerUnitOther { get; set; }
        public string Options { get; set; }
        public int HoursNoGreaterThan { get; set; }
    }
}
