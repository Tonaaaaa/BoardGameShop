using AutoMapper;
using BoardGameShop.Api.Entities;
using BoardGameShop.Api.Models.CartItem;
using BoardGameShop.Api.Models.Category;
using BoardGameShop.Api.Models.Coupon;
using BoardGameShop.Api.Models.Order;
using BoardGameShop.Api.Models.OrderItem;
using BoardGameShop.Api.Models.Product;
using BoardGameShop.Api.Models.ProductImage;
using BoardGameShop.Api.Models.Review;
using BoardGameShop.Api.Models.User;

namespace BoardGameShop.Api.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CartItem, CartItemDTO>();
            CreateMap<CreateCartItemDTO, CartItem>();
            CreateMap<UpdateCartItemDTO, CartItem>();

            CreateMap<Category, CategoryDTO>();
            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<UpdateCategoryDTO, Category>();

            CreateMap<Coupon, CouponDTO>();
            CreateMap<CreateCouponDTO, Coupon>();
            CreateMap<UpdateCouponDTO, Coupon>();

            CreateMap<Order, OrderDTO>();
            CreateMap<CreateOrderDTO, Order>();
            CreateMap<UpdateOrderDTO, Order>();

            CreateMap<OrderItem, OrderItemDTO>();
            CreateMap<CreateOrderItemDTO, OrderItem>();

            CreateMap<Product, ProductDTO>();
            CreateMap<CreateProductDTO, Product>();
            CreateMap<UpdateProductDTO, Product>();

            CreateMap<ProductImage, ProductImageDTO>();
            CreateMap<CreateProductImageDTO, ProductImage>();
            CreateMap<UpdateProductImageDTO, ProductImage>();

            CreateMap<Review, ReviewDTO>();
            CreateMap<CreateReviewDTO, Review>();
            CreateMap<UpdateReviewDTO, Review>();

            CreateMap<User, UserDTO>();
            CreateMap<CreateUserDTO, User>();
            CreateMap<UpdateUserDTO, User>();
        }
    }
}