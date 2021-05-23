namespace DragonSouvenirs.Data.Configurations
{
    using DragonSouvenirs.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class FavouriteProductConfiguration : IEntityTypeConfiguration<FavouriteProduct>
    {
        public void Configure(EntityTypeBuilder<FavouriteProduct> favouriteProduct)
        {
            favouriteProduct
                .HasKey(fp => new { fp.UserId, fp.ProductId });

            favouriteProduct
                .HasOne(fp => fp.User)
                .WithMany(u => u.FavouriteProducts)
                .HasForeignKey(fp => fp.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            favouriteProduct
                .HasOne(fp => fp.Product)
                .WithMany(p => p.FavouriteProducts)
                .HasForeignKey(fp => fp.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
