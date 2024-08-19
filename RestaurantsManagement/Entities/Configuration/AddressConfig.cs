using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AddressConfig : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.Property(x => x.City).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Street).IsRequired().HasMaxLength(50);
    }
}
