using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandyMarket.Models
{
    public class UserWithCandyInfo
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public IEnumerable<Candy> Candy { get; set; }
        //public string Candy { get; set; }
    }
}
