using ApiApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiApplication.Configuration
{
    public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
    {
        public void Configure(EntityTypeBuilder<Phone> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Company)
                .WithMany(u => u.Phones)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(e => e.Orders)
               .WithMany(u => u.Phones);

        }
    }
}
