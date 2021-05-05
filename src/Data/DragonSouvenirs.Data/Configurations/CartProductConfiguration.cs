namespace DragonSouvenirs.Data.Configurations
{
    using DragonSouvenirs.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CartProductConfiguration : IEntityTypeConfiguration<CartProduct>
    {
        public void Configure(EntityTypeBuilder<CartProduct> cartProduct)
        {
            cartProduct
                .HasKey(k => new { k.ProductId, k.CartId });

            cartProduct
                .HasOne(cp => cp.Product)
                .WithMany(p => p.CartProducts)
                .HasForeignKey(cp => cp.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            cartProduct
                .HasOne(cp => cp.Cart)
                .WithMany(c => c.CartProducts)
                .HasForeignKey(cp => cp.CartId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
