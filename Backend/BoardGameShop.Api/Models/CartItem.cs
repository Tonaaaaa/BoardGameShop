using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGameShop.Api.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public string? SessionId { get; set; }
        public int? CustomerId { get; set; }
        public User? Customer { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}