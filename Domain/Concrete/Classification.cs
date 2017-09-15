using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Concrete.Common;

namespace Domain.Concrete
{
    public class Classification
    {
        public decimal SGSpecificGravity { get; set; }
        public string ProductType { get; set; }
        public int ProductionTargetNumber { get; set; }
        public string ProductionTargetUnit { get; set; }
        public string MetallurgicalTestWork { get; set; }
        public string ClassifierType { get; set; }
        public string ClassifierTypeOther { get; set; }
        public int FlowRateGPM { get; set; }
        public int SolidsPercentOverflow { get; set; }
        public int SolidsPercentUnderflow { get; set; }
        public string EstimatedCirculatingLoad { get; set; }
        public string PressureDropTargetKPA { get; set; }
        public string CycloneEstimatedDiameter { get; set; }
        public string CycloneEstimatedDiameterUnit { get; set; }
        public int Feed80PercentPassingExpectedMicron { get; set; }
        public int Products80PercentPassingExpectedMicron { get; set; }
        public bool MotorIncluded { get; set; }

        public Motor Motor { get; set; }
    }
}
