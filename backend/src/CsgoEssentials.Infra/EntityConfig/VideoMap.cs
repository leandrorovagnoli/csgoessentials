using CsgoEssentials.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsgoEssentials.Infra.EntityConfig
{
    public class VideoMap : IEntityTypeConfiguration<Video>
    {
        public void Configure(EntityTypeBuilder<Video> builder)
        {
            builder.ToTable("Video");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Name).IsUnique(true);
        }
    }
}
