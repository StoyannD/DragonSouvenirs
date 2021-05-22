namespace DragonSouvenirs.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DragonSouvenirs.Data.Common.Repositories;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class CartService : ICartService
    {
        private readonly IDeletableEntityRepository<Cart> cartRepository;
        private readonly IDeletableEntityRepository<CartProduct> cartProductRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IDeletableEntityRepository<Product> productRepository;

        public CartService(
            IDeletableEntityRepository<Cart> cartRepository,
            IDeletableEntityRepository<CartProduct> cartProductRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IDeletableEntityRepository<Product> productRepository)
        {
            this.cartRepository = cartRepository;
            this.cartProductRepository = cartProductRepository;
            this.userRepository = userRepository;
            this.productRepository = productRepository;
        }

        public async Task<IEnumerable<T>> GetCartProductsAsync<T>(string userId)
        {
            var cart = await this.cartProductRepository
                .All()
                .Where(c => c.Cart.UserId == userId)
                .To<T>()
                .ToListAsync();

            return cart;
        }

        public async Task<decimal> GetCartTotalPriceAsync(string userId)
        {
            var totalPrice = await this.cartProductRepository
                .All()
                .Include(cp => cp.Cart)
                .Where(cp => cp.Cart.UserId == userId)
                .SumAsync(cp => cp.Quantity * cp.Product.Price);

            return totalPrice;
        }

        public async Task AddProductToCartAsync(string userId, int productId, int quantity)
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

            var cartProduct = new CartProduct();

            var product = await this.productRepository.All().FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
            {
                throw new ArgumentException();
            }

            if (product.Quantity < quantity)
            {
                return;
            }

            // Check if a deleted CardProduct with the same product exists.
            if (await this.cartProductRepository
                    .AllWithDeleted()
                    .Where(cp => cp.Cart.UserId == userId)
                    .AnyAsync(cp => cp.ProductId == productId))
            {
                cartProduct = await this.cartProductRepository
                    .AllWithDeleted()
                    .FirstOrDefaultAsync(cp => cp.ProductId == productId);

                if (cartProduct.IsDeleted)
                {
                    cartProduct.Quantity = quantity;
                    cartProduct.IsDeleted = false;
                }
                else
                {
                    cartProduct.Quantity += quantity;
                }
            }
            else
            {
                cartProduct.Cart = user.Cart;
                cartProduct.Quantity = quantity;
                cartProduct.CreatedOn = DateTime.UtcNow;
                cartProduct.ProductId = productId;

                await this.cartProductRepository.AddAsync(cartProduct);
            }

            // product.Quantity -= quantity;
            // await this.productRepository.SaveChangesAsync();
            await this.cartProductRepository.SaveChangesAsync();
        }

        public async Task DeleteProductFromCartAsync(string userId, int productId)
        {
            var cartProduct = await this.cartProductRepository
                .AllWithDeleted()
                .FirstOrDefaultAsync(
                    cp => cp.Cart.UserId == userId
                          && cp.ProductId == productId);

            if (cartProduct == null)
            {
                // TODO add message
                throw new NullReferenceException();
            }

            this.cartProductRepository
                .Delete(cartProduct);

            await this.cartProductRepository.SaveChangesAsync();
        }

        public async Task EditProductInCartAsync(string userId, int productId, int quantity)
        {
            var cartProduct = await this.cartProductRepository
                .All()
                .FirstOrDefaultAsync(
                    pc => pc.Cart.UserId == userId
                          && pc.ProductId == productId);

            if (cartProduct == null)
            {
                // TODO add message
                throw new NullReferenceException();
            }

            var product = await this.productRepository
                .All()
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product.Quantity < quantity)
            {
                return;
            }

            cartProduct.Quantity = product.Quantity < quantity
                ? product.Quantity
                : quantity;

            await this.cartProductRepository.SaveChangesAsync();
        }

        public async Task<T> GetCartByIdAsync<T>(string id)
        {
            var cart = await this.cartRepository
                .All()
                .Where(c => c.User.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return cart;
        }

        public async Task<bool> UserHasProductsInCart(string userId)
        {
            var hasProducts = await this.cartProductRepository
                .All()
                .Where(cp => cp.Cart.UserId == userId)
                .AnyAsync();

            return hasProducts;
        }
    }
}
