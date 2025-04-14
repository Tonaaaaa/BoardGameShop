using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGameShop.Api.Models.ProductImage
{
    public class CreateProductImageDTO
    {
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public string AltText { get; set; }
        public bool IsMain { get; set; }
        public int DisplayOrder { get; set; }
    }
}