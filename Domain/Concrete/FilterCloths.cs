using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Concrete.Common;

namespace Domain.Concrete
{
    public class FilterCloths
    {
        public Product Product { get; set; }

        public string Quantity { get; set; }
        public string Unit { get; set; }

        public string Length { get; set; }
        public string LengthOther { get; set; }

        public string Volume { get; set; }
        public string VolumeOther { get; set; }

        public string Weight { get; set; }
        public string WeightOther { get; set; }

        public string Diameter { get; set; }
        public string DiameterOther { get; set; }

        public string Material { get; set; }
        public string MaterialOther { get; set; }

        public string Size { get; set; }
    }
}
