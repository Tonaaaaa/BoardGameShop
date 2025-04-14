using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardGameShop.Api.Models.Category;

namespace BoardGameShop.Api.Services
{
    public interface ICategoryService
    {
        Task<CategoryDTO> CreateAsync(CreateCategoryDTO request);
    }
}