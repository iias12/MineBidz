using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Concrete.Common;

namespace Domain.Concrete
{
    public class OtherEquipment
    {
        public string WorkIndexKWH { get; set; }
        public string SGSpecificGravity { get; set; }
        public string OreType { get; set; }
        public string ProductionTargetNumber { get; set; }
        public string ProductionTargetUnit { get; set; }
        public string MetallurgicalTestWork { get; set; }
        public bool MotorIncluded { get; set; }

        public Motor Motor { get; set; }
    }
}
