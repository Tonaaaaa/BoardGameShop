using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGameShop.Api.Models.Review
{
    public class UpdateReviewDTO
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public bool IsApproved { get; set; }
        public string Reply { get; set; }
    }
}