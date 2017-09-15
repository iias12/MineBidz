using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class Pump
    {
        public decimal SGSpecificGravity { get; set; }
        public string ProductType { get; set; }
        public int ProductionTargetNumber { get; set; }
        public string ProductionTargetUnit { get; set; }
        public bool MetallurgicalTestWork { get; set; }
        public string PumpType { get; set; }
        public string OperatingConditions { get; set; }
        public string FluidType { get; set; }
        public int MaxSolidSize { get; set; }
        public int SolidsPercent { get; set; }
        public string FluidPH { get; set; }
        public int Temerature { get; set; }
        public string TemperatureUnit { get; set; }
        public int FlowRateGPM { get; set; }
        public bool MotorIncluded { get; set; }
        public int MotorVoltage { get; set; }
        public string EnclosureType { get; set; }
        public int Phase { get; set; }
        public int RpmSpeed { get; set; }
        public int PowerHP { get; set; }
        public int ProjectElevation { get; set; }
        public string ProjectElevationUnit { get; set; }
        public int Hertz { get; set; }
        public string OtherInfo { get; set; }
        public int HeadTDH { get; set; }
        public string HeadTDHUnit { get; set; }
        public string HeadTDHUnitOther { get; set; }
        public int SuctionLift { get; set; }
        public string SuctionLiftUnit { get; set; }
        public string SuctionLiftUnitOther { get; set; }
        public int OperatingTimePercent { get; set; }
        public string Materials { get; set; }

    }
}
