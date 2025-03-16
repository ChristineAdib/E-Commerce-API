using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Data.Configuration
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(P => P.Category)
                   .WithMany(C=>C.Products)
                   .HasForeignKey(P => P.CategoryId);

            builder.Property(P => P.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(P => P.Description).IsRequired();
            builder.Property(P => P.ImageUrl).IsRequired();

            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
        }
    }
}
