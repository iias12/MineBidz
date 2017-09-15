using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete.Common
{
    public class Motor
    {
        public int MotorVoltage { get; set; }
        public string EnclosureType { get; set; }
        public int Phase { get; set; }
        public int RpmSpeed { get; set; }
        public int PowerHP { get; set; }
        public int ProjectElevation { get; set; }
        public string ProjectElevationUnit { get; set; }
        public int Hertz { get; set; }
    }
}
