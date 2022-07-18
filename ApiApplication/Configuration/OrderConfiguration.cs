using ApiApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
       
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.User)
                .WithMany(u => u.Orders)
                .OnDelete(DeleteBehavior.SetNull)
                .HasForeignKey(e => e.UserId);
            builder.HasMany(e => e.Phones)
               .WithMany(u => u.Orders);
                
        }
    }
}
