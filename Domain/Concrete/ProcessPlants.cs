using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Concrete.Common;

namespace Domain.Concrete
{
    public class ProcessPlants
    {
        public string SGSpecificGravity { get; set; }
        public string ProductType { get; set; }
        public int ProductionTargetNumber { get; set; }
        public string ProductionTargetUnit { get; set; }
        public string MetallurgicalTestWork { get; set; }
        public string SettingType { get; set; }
        public string ProcessType { get; set; } 
        public string OreType { get; set; }
        public string OreTypeOther { get; set; }
        public string WorkIndexkWht { get; set; }
        public string ProcessCondition { get; set; }
        public string CircuitType { get; set; }

        public string Feed80PercentPassingExpectedMM { get; set; }
        public string Product80PercentPassingExpectedMM { get; set; }
        public string MillPercentOfCriticalSpeed { get; set; }
        public string PercentFillingDegreeExpected { get; set; }

        public bool MotorIncluded { get; set; }

        public Motor Motor { get; set; }
    }
}
