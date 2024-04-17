using Core.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        void IEntityTypeConfiguration<Product>.Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired();

            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");      // decimal number with a precision of 18 digits and a scale of 2

            builder.Property(p => p.PictureUrl).IsRequired();

            builder.HasOne(b => b.ProductBrand).WithMany()  //Product has one - to - many relationship with ProductBrand
                .HasForeignKey(p => p.ProductBrandId);

            builder.HasOne(t => t.ProductType).WithMany()
                .HasForeignKey(p => p.ProductTypeId);

        }
    }
}
