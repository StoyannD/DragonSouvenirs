namespace DragonSouvenirs.Data.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DragonSouvenirs.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> productCategory)
        {
            productCategory
                .HasKey(c => new { c.ProductId, c.CatgoryId });

            productCategory
                .HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CatgoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            productCategory
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
