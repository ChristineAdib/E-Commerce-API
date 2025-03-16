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
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasMany(C => C.Products)
                   .WithOne();

            //builder.HasMany(C=>C.Children)
            //       .WithOne();

            //builder.HasOne(C => C.Parent)
            //       .WithMany(C => C.Children);

            builder.Property(C => C.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(C => C.Description)
                   .IsRequired();


        }
    }
}
