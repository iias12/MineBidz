using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Concrete.Common;

namespace Domain.Concrete
{
    public class Screen
    {
        public string SGSpecificGravity { get; set; }
        public string ProductType { get; set; }
        public int ProductionTargetNumber { get; set; }
        public string ProductionTargetUnit { get; set; }
        public bool MetallurgicalTestWork { get; set; }
        public string ScreenType { get; set; }
        public string ScreenTypeOther { get; set; }
        public string MotionType { get; set; }
        public string MotionTypeOther { get; set; }

        public string PercentMoistureContentInOre { get; set; }
        public string Feed80PercentPassingExpectedMM { get; set; }
        public string Product80PercentPassingExpectedMM { get; set; }
        public string DeckType { get; set; }
        public string DeckTypeOther { get; set; }
        public bool DustControl { get; set; }
        public string KnownSizeOfScreenRequired1 { get; set; }
        public string KnownSizeOfScreenRequired2 { get; set; }
        public string KnownSizeOfScreenRequiredUnit { get; set; }

        public bool MotorIncluded { get; set; }

        public Motor Motor { get; set; }
    }
}
