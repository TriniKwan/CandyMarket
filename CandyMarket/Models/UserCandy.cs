using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandyMarket.Models
{
    public class UserCandy
    {
        public int UserId { get; set; }
        public int CandyId { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
