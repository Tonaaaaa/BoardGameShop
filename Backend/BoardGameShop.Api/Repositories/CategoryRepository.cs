using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BoardGameShop.Api.Data;
using BoardGameShop.Api.Entities;
using BoardGameShop.Api.Models.Category;
using BoardGameShop.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace BoardGameShop.Api.Repositories
{
    public class CategoryRepository : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> CreateAsync(CreateCategoryDTO request)
        {
            if (await _context.Categories.AnyAsync(c => c.Slug == request.Slug))
                throw new InvalidOperationException($"Category with slug '{request.Slug}' already exists.");

            if (request.ParentId.HasValue)
            {
                var parent = await _context.Categories.FindAsync(request.ParentId);
                if (parent == null)
                    throw new KeyNotFoundException($"Parent category with ID {request.ParentId} not found.");
            }

            var category = _mapper.Map<Category>(request);
            category.CreatedAt = DateTime.UtcNow;
            category.IsActive = true;

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return _mapper.Map<CategoryDTO>(category);
        }
    }
}