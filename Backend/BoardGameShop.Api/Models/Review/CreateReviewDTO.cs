using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGameShop.Api.Models.Review
{
    public class CreateReviewDTO
    {
        public int ProductId { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}