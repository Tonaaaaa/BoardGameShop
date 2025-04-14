using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGameShop.Api.Models.CartItem
{
    public class CreateCartItemDTO
    {
        public string SessionId { get; set; }
        public int? CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}