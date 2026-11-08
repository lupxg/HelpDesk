using HelpDesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelpDesk.Infrastructure.Persistence.Configurations;

public sealed class SlaPolicyConfiguration : IEntityTypeConfiguration<SlaPolicy>
{
    public void Configure(EntityTypeBuilder<SlaPolicy> builder)
    {
        builder.ToTable("SlaPolicies");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Priority).IsRequired().HasConversion<string>().HasMaxLength(30);

        builder.Property(t => t.ResponseTimeMinutes).IsRequired();

        builder.Property(t => t.ResolutionTimeMinutes).IsRequired();

        builder.ToTable("SlaPolicies", table =>
        {
            table.HasCheckConstraint("CK_SlaPolicies_ResponseTimeMinutes", "\"ResponseTimeMinutes\" > 0");

            table.HasCheckConstraint("CK_SlaPolicies_ResolutionTimeMinutes", "\"ResolutionTimeMinutes\" > 0");
        });
    }
}
