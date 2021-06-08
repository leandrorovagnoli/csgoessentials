using CsgoEssentials.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CsgoEssentials.Infra.EntityConfig
{
    public class PlayerAttributeMap : IEntityTypeConfiguration<PlayerAttribute>
    {
        public void Configure(EntityTypeBuilder<PlayerAttribute> builder)
        {
            builder.ToTable("PlayerAttribute");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Name).IsUnique(true);
        }
    }
}
