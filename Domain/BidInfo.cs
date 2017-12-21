using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class BidInfo
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string BidName { get; set; }
        [Required(ErrorMessage = "Closing Dte is required")]
        public DateTime BidEndDate { get; set; }
        public DateTime BidStartDate { get; set; }
    }
}
