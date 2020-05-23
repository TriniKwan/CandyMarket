using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandyMarket.Models
{
    public class UserWithOldestCandy
    {
        public DateTime DateAdded { get; set; }
        public string CandyType { get; set; }
        public int CandyId { get; set; }
        public int UserCandyId { get; set; }
    }
}
