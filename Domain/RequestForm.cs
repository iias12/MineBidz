using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class RequestForm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FormName { get; set; }
        public string ClassName { get; set; }
        public int SubcategoryId { get; set; }
        public bool Implemented { get; set; }
    }
}
