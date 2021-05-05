using Microsoft.AspNetCore.Identity;

namespace DragonSouvenirs.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DragonSouvenirs.Data.Common.Repositories;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;
    using DragonSouvenirs.Web.ViewModels.Cart;
    using Microsoft.EntityFrameworkCore;

    public class CartService : ICartService
    {
        private readonly IDeletableEntityRepository<Cart> cartRepository;
        private readonly IDeletableEntityRepository<CartProduct> cartProductRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public CartService(
            IDeletableEntityRepository<Cart> cartRepository,
            IDeletableEntityRepository<CartProduct> cartProductRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.cartRepository = cartRepository;
            this.cartProductRepository = cartProductRepository;
            this.userRepository = userRepository;
        }

        public async Task<IEnumerable<T>> GetCartProductsAsync<T>(string userId)
        {
            var cart = await this.cartProductRepository
                .AllWithDeleted()
                .Where(c => c.Cart.UserId == userId)
                .To<T>()
                .ToListAsync();

            return cart;
        }

        public async Task AddProductToCartAsync(string userId, int productId)
        {
            var user = await this.userRepository
                .AllWithDeleted()
                .Include(u => u.Cart)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user.Cart == null)
            {
                var cart = new Cart()
                {
                    CreatedOn = DateTime.UtcNow,
                    UserId = user.Id,
                };

                await this.cartRepository.AddAsync(cart);
                await this.cartRepository.SaveChangesAsync();
            }

            var cartProduct = new CartProduct()
            {
                Cart = user.Cart,
                Quantity = 1,
                CreatedOn = DateTime.UtcNow,
                ProductId = productId,
            };

            await this.cartProductRepository.AddAsync(cartProduct);
            await this.cartProductRepository.SaveChangesAsync();
        }
    }
}
