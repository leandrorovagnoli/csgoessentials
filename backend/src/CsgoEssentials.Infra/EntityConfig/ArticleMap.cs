using CsgoEssentials.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;


namespace CsgoEssentials.Infra.EntityConfig
{
    class ArticleMap : IEntityTypeConfiguration<Map>
    {
        public void Configure(EntityTypeBuilder<Map> builder)
        {
            builder.ToTable("Map");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.MapName).IsUnique(true);
        }
    }
}

