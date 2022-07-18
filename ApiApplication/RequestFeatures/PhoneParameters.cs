using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class PhoneParameters : RequestParameters
    {
        public PhoneParameters()
        {
            OrderBy = "name";
        }
        public uint MinPrice { get; set; }//for filtering
        public uint MaxPrice { get; set; } = int.MaxValue;
        public bool ValidPriceRange 
        {
            get
            {
                return MaxPrice > MinPrice;
            }
        }
        public string SearchTerm { get; set; }//for searching


        //public bool ValidPriceRange => MaxPrice > MinPrice;
    }
}
