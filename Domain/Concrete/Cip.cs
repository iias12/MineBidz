using Domain.Concrete.Common;

namespace Domain.Concrete
{
    public class Cip
    {
        public string ProductType { get; set; }
        public string MetallurgicalTestWork { get; set; }
        public string Mobility { get; set; }
        public string FlowGPM { get; set; }
        public string PercentSolids { get; set; }
        public string ResidenceTimeHours { get; set; }
        public string TankStyle { get; set; }
        public string TankStyleUnit { get; set; }
        public string TankRubberLined { get; set; }
        public string TailsTankWithSafetyScreen { get; set; }

        public bool MotorIncluded { get; set; }

        public Motor Motor { get; set; }
        public TankScreenCommon TankScreenCommon { get; set; }
    }
}
