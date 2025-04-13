using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGameShop.Api.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsMain { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}