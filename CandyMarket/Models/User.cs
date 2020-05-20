using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandyMarket.Models
{
    public class User
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public List<string> Candy { get; set; }
    }
}
