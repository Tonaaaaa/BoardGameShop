using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardGameShop.Api.Models.Category;
using FluentValidation;

namespace BoardGameShop.Api.Validators
{
    public class CreateCategoryDTOValidator : AbstractValidator<CreateCategoryDTO>
    {
        public CreateCategoryDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MinimumLength(2).WithMessage("Category name must be at least 2 characters.")
                .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug is required.")
                .MinimumLength(2).WithMessage("Slug must be at least 2 characters.")
                .MaximumLength(100).WithMessage("Slug cannot exceed 100 characters.")
                .Matches("^[a-z0-9-]+$").WithMessage("Slug can only contain lowercase letters, numbers, and hyphens.");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be greater than or equal to 0.");
        }
    }
}