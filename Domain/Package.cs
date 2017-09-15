using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Package
    {
        public int PackageId {get; set;}
        public int PackageTypeId {get; set;}
        public decimal PackagePrice{get; set;} 
        public int PackageTermMonth {get; set;} 
        public string PackageName {get; set;}
        public string PackageText {get; set;}
        public string Image {get; set;}
    }
}
