using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGameShop.Api.Models.Category
{
    public class CreateCategoryDTO
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public int? ParentId { get; set; }
        public int DisplayOrder { get; set; }
    }
}