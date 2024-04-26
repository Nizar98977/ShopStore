using Core.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ProductEntityTypeConfigurations : IEntityTypeConfiguration<Product>, IEntityTypeConfiguration<ProductType>, IEntityTypeConfiguration<ProductBrand>
    {
        public void Configure(EntityTypeBuilder<ProductType> builder)
        {
            builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd(); ;
        }
        public void Configure(EntityTypeBuilder<ProductBrand> builder)
        {
            builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd(); ;
        }

        void IEntityTypeConfiguration<Product>.Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd(); ;
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired();

            builder.Property(p => p.Price).HasPrecision(18, 2);// decimal number with a precision of 18 digits and a scale of 2

            builder.Property(p => p.PictureUrl).IsRequired();

            builder.HasOne(b => b.ProductBrand).WithMany()  //Product has one - to - many relationship with ProductBrand
                .HasForeignKey(p => p.ProductBrandId);

            builder.HasOne(t => t.ProductType).WithMany()
                .HasForeignKey(p => p.ProductTypeId);
        }
    }
}
