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
        public IEnumerable<string> Candy { get; set; }
        //public string Candy { get; set; }
        public int CandyId { get; set; }
    }
}
