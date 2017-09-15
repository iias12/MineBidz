using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class BidInfo
    {
        public int Id { get; set; }
        public string BidName { get; set; }
        public DateTime BidEndDate { get; set; }
        public DateTime BidStartDate { get; set; }
    }
}
