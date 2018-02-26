using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class RequestInfoFilter
    {
        public int CategoryId { get; set; }
        public string CountryCode { get; set; }
        public string StateProvinceCode { get; set; }
        public string Title { get; set; }
    }
}
