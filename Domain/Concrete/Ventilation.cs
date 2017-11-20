using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Concrete.Common;

namespace Domain.Concrete
{
    public class Ventilation
    {
        public string TubingType { get; set; }
        public string TubingTypeOther { get; set; }

        public string Length { get; set; }
        public string LengthUnit { get; set; }
        public string LengthUnitOther { get; set; }

        public string Diameter { get; set; }
        public string DiameterUnit { get; set; }
        public string DiameterUnitOther { get; set; }

        public string CouplingsQuantity { get; set; }
        public string CouplingsQuantityUnit { get; set; }
        public string CouplingsQuantityUnitOther { get; set; }

        public string FansQuantity { get; set; }
        public string FansQuantityUnit { get; set; }
        public string FansQuantityUnitOther { get; set; }

        public string Scfm { get; set; }

        public string Type { get; set; }
        public string TypeOther { get; set; }

        public string Silencer { get; set; }
        public string SilencerOther { get; set; }

        public string Accessories { get; set; }
        public string AccessoriesOther { get; set; }

        public string Damper { get; set; }
        public string DamperOther { get; set; }

        public string HeatersBtuNumber { get; set; }
        public bool HeatersBtuNumberYesNo { get; set; }

        public string AirConditioningTons { get; set; }
        public bool AirConditioningTonsYesNo { get; set; } 

        public bool MotorIncluded { get; set; }

        public Motor Motor { get; set; }
    }
}
