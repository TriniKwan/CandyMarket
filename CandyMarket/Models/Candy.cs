using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandyMarket.Models
{
    public class Candy
    {
        public int CandyId { get; set; }
        public string CandyType { get; set; }
        public DateTime DateAdded { get; set; }

    }

    public class CandyWithAllInfo
    {
        public int CandyId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Manufacturer { get; set; }
    }
}
