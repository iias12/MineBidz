using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class Vehicle
    {
        public string Type { get; set; }
        public string TypeOther { get; set; }
        public string HoursNoGreaterThen { get; set; }
        public string NoMilesGreaterThan { get; set; }
        public string Drive { get; set; }
        public string DriveOther { get; set; }
        public string Cab { get; set; }
        public string CabOther { get; set; }
        public string Size { get; set; }
        public string SizeOther { get; set; }
        public string Accessories { get; set; }
        public string AccessoriesOther { get; set; }
        public int Quantity { get; set; }

    }
}
