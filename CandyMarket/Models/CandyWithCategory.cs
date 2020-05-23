using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandyMarket.Models
{
    public class CandyWithCategory
    {
        public string CandyType { get; set; }
        public int CandyId { get; set; }
        public string Type { get; set; }
        public int CategoryId { get; set; }
    }
}
