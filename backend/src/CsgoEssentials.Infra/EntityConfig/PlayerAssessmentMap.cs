using CsgoEssentials.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CsgoEssentials.Infra.EntityConfig
{
    public class PlayerAssessmentMap : IEntityTypeConfiguration<PlayerAssessment>
    {
        public void Configure(EntityTypeBuilder<PlayerAssessment> builder)
        {
            builder.ToTable("PlayerAssessment");

            builder.HasKey(x => x.Id);
        }
    }
}
