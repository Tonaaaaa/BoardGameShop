using BoardGameShop.Api.Data;
using BoardGameShop.Api.Entities;
using BoardGameShop.Api.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace BoardGameShop.Api.Repositories
{
    public class ProductRepository : IProductService
    {
        private readonly AppDbContext _context;
        private readonly ICloudinaryService _cloudinaryService;

        public ProductRepository(AppDbContext context, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Where(p => p.IsActive)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .ToListAsync();
        }

        public async Task<Product> GetBySlugAsync(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                throw new ArgumentException("Slug cannot be null or empty.");

            var product = await _context.Products
                .Where(p => p.IsActive && p.Slug == slug)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync();

            if (product == null)
                throw new KeyNotFoundException($"Product with slug {slug} not found.");

            return product;
        }

        public async Task<Product> CreateAsync(Product product, IFormFile image)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Product name cannot be null or empty.");

            product.Slug = GenerateSlug(product.Name);
            product.IsActive = true;
            product.CreatedAt = DateTime.UtcNow;

            var existingProduct = await _context.Products
                .AnyAsync(p => p.Slug == product.Slug);
            if (existingProduct)
                throw new InvalidOperationException($"A product with slug '{product.Slug}' already exists.");

            if (image != null)
            {
                var imageUrl = await _cloudinaryService.UploadImageAsync(image);
                if (string.IsNullOrEmpty(imageUrl))
                    throw new Exception("Failed to upload image to Cloudinary.");
                product.ImageUrl = imageUrl;
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateAsync(int id, Product product, IFormFile image)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Product name cannot be null or empty.");

            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
                throw new KeyNotFoundException($"Product with id {id} not found.");

            existingProduct.Name = product.Name;
            existingProduct.Slug = GenerateSlug(product.Name);
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.OriginalPrice = product.OriginalPrice;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.MetaTitle = product.MetaTitle;
            existingProduct.MetaDescription = product.MetaDescription;
            existingProduct.IsActive = product.IsActive;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            var duplicateProduct = await _context.Products
                .AnyAsync(p => p.Slug == existingProduct.Slug && p.Id != id);
            if (duplicateProduct)
                throw new InvalidOperationException($"A product with slug '{existingProduct.Slug}' already exists.");

            if (image != null)
            {
                var imageUrl = await _cloudinaryService.UploadImageAsync(image);
                if (string.IsNullOrEmpty(imageUrl))
                    throw new Exception("Failed to upload image to Cloudinary.");
                existingProduct.ImageUrl = imageUrl;
            }

            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<Product> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with id {id} not found.");

            // Soft delete
            product.IsActive = false;
            product.UpdatedAt = DateTime.UtcNow;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        private string GenerateSlug(string name)
        {
            string slug = name.Normalize(NormalizationForm.FormD);
            var regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            slug = regex.Replace(slug, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');

            slug = slug.ToLowerInvariant();
            slug = Regex.Replace(slug, "[^a-z0-9\\-]", "-");
            slug = Regex.Replace(slug, "-+", "-").Trim('-');

            return slug;
        }
    }
}