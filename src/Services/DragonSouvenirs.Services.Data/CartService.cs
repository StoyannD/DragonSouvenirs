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

        // Get the products in the user's shopping cart
        public async Task<IEnumerable<T>> GetCartProductsAsync<T>(string userId)
        {
            var cart = await this.cartProductRepository
                .All()
                .Where(c => c.Cart.UserId == userId)
                .To<T>()
                .ToListAsync();

            return cart;
        }

        // Get the products cart price with the discount
        public async Task<decimal> GetCartTotalPriceAsync(string userId, decimal personalDiscountPercentage)
        {
            var totalPrice = await this.cartProductRepository
                .All()
                .Include(cp => cp.Cart)
                .Where(cp => cp.Cart.UserId == userId)
                .SumAsync(cp => cp.Quantity * (cp.Product.DiscountPrice ?? cp.Product.Price));

            totalPrice *= personalDiscountPercentage == 0
                ? 1
                : 1 - (personalDiscountPercentage / 100);

            return totalPrice;
        }

        // Add a product to the user's shopping cart
        public async Task<bool> AddProductToCartAsync(string userId, int productId, int quantity)
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
                return false;
            }

            if (product.Quantity < quantity)
            {
                return false;
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

            // Check if the quantity exceeds total product quantity
            if (cartProduct.Quantity > product.Quantity)
            {
                cartProduct.Quantity = product.Quantity;
            }

            await this.cartProductRepository.SaveChangesAsync();
            return true;
        }

        // Delete a product from the user's shopping cart
        public async Task<bool> DeleteProductFromCartAsync(string userId, int productId)
        {
            var cartProduct = await this.GetProductFromCart(userId, productId);

            if (cartProduct == null)
            {
                return false;
            }

            this.cartProductRepository
                .Delete(cartProduct);

            await this.cartProductRepository.SaveChangesAsync();
            return true;
        }

        // Edit a product in the user's shopping cart
        public async Task<bool> EditProductInCartAsync(string userId, int productId, int quantity)
        {
            var cartProduct = await this.GetProductFromCart(userId, productId);

            if (cartProduct == null)
            {
                return false;
            }

            var product = await this.productRepository
                .All()
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product.Quantity < quantity)
            {
                return false;
            }

            // Set the quantity to the max available product quantity in stock
            cartProduct.Quantity = product.Quantity < quantity
                ? product.Quantity
                : quantity;

            await this.cartProductRepository.SaveChangesAsync();
            return true;
        }

        // Get the shopping cart by Id
        public async Task<T> GetCartByIdAsync<T>(string id)
        {
            var cart = await this.cartRepository
                .All()
                .Where(c => c.User.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return cart;
        }

        // Check if user's shopping cart has any products(NOT DELETED)
        public async Task<bool> UserHasProductsInCart(string userId)
        {
            var hasValidCart = await this.cartRepository
                .All()
                .AnyAsync(c => c.UserId == userId);

            if (hasValidCart)
            {
                hasValidCart = await this.cartProductRepository
                    .All()
                    .Where(cp => cp.Cart.UserId == userId && !cp.Product.IsDeleted)
                    .AnyAsync();
            }

            return hasValidCart;
        }

        // Get the product from the user's shopping cart
        private async Task<CartProduct> GetProductFromCart(string userId, int productId)
        {
            return await this.cartProductRepository
                .AllWithDeleted()
                .FirstOrDefaultAsync(
                    cp => cp.Cart.UserId == userId
                          && cp.ProductId == productId);
        }
    }
}
