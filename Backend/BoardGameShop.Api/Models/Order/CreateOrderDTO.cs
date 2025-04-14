using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGameShop.Api.Models.Order
{
    public class CreateOrderDTO
    {
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string CouponCode { get; set; }
        public string Notes { get; set; }
        public ICollection<CreateOrderItemDTO> OrderItems { get; set; }
    }
}