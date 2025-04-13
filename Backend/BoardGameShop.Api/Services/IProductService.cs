using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BoardGameShop.Api.Entities;
using Microsoft.AspNetCore.Http;

namespace BoardGameShop.Api.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetBySlugAsync(string slug);
        Task<Product> CreateAsync(Product product, IFormFile image);
        Task<Product> UpdateAsync(int id, Product product, IFormFile image);
        Task<Product> DeleteAsync(int id);
    }
}