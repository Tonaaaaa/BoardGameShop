using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using BoardGameShop.Api.Data;
using BoardGameShop.Api.Entities;
using BoardGameShop.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace BoardGameShop.Api.Repositories
{
    public class CategoryRepository : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories
                .ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                throw new KeyNotFoundException($"Category with id {id} not found.");

            return category;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            if (string.IsNullOrWhiteSpace(category.Name))
                throw new ArgumentException("Category name cannot be null or empty.");

            category.Slug = !string.IsNullOrWhiteSpace(category.Slug) ? category.Slug : GenerateSlug(category.Name);
            category.CreatedAt = DateTime.UtcNow;

            var existingCategory = await _context.Categories
                .AnyAsync(c => c.Name == category.Name || c.Slug == category.Slug);
            if (existingCategory)
                throw new InvalidOperationException($"A category with name '{category.Name}' or slug '{category.Slug}' already exists.");

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateAsync(int id, Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            if (string.IsNullOrWhiteSpace(category.Name))
                throw new ArgumentException("Category name cannot be null or empty.");

            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null)
                throw new KeyNotFoundException($"Category with id {id} not found.");

            existingCategory.Name = category.Name;
            existingCategory.Slug = !string.IsNullOrWhiteSpace(category.Slug) ? category.Slug : GenerateSlug(category.Name);
            existingCategory.Description = category.Description;
            existingCategory.UpdatedAt = DateTime.UtcNow;

            var duplicateCategory = await _context.Categories
                .AnyAsync(c => (c.Name == category.Name || c.Slug == existingCategory.Slug) && c.Id != id);
            if (duplicateCategory)
                throw new InvalidOperationException($"A category with name '{category.Name}' or slug '{existingCategory.Slug}' already exists.");

            _context.Categories.Update(existingCategory);
            await _context.SaveChangesAsync();
            return existingCategory;
        }

        public async Task<Category> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with id {id} not found.");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return category;
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