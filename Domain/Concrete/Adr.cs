using Domain.Concrete.Common;

namespace Domain.Concrete
{
    public class Adr
    {
        public string ProductType { get; set; }
        public string MetallurgicalTestWork { get; set; }
        public string FlowGPM { get; set; }
        public string CarbonColumnSolutionFeedGradeGold { get; set; }
        public string CarbonColumnSolutionFeedGradeGoldUnit { get; set; }
        public string CarbonColumnSolutionFeedGradeSilver { get; set; }
        public string CarbonColumnSolutionFeedGradeSilverUnit { get; set; }
        public bool MotorIncluded { get; set; }

        public Motor Motor { get; set; }
        public TankScreenCommon TankScreenCommon { get; set; }
    }
}
