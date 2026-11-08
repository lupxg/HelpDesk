using HelpDesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelpDesk.Infrastructure.Persistence.Configurations;

public sealed class TicketHistoryConfiguration : IEntityTypeConfiguration<TicketHistory>
{
    public void Configure(EntityTypeBuilder<TicketHistory> builder)
    {
        builder.ToTable("TicketHistory");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.TicketId).IsRequired();

        builder.Property(t => t.ActorId).IsRequired(false);

        builder.Property(t => t.EventType).IsRequired().HasMaxLength(100);

        builder.Property(t => t.OldValue).HasMaxLength(2_000);

        builder.Property(t => t.NewValue).HasMaxLength(2_000);

        builder.Property(t => t.CreatedAtUtc).IsRequired();

        builder.Property(t => t.UpdatedAtUtc).IsRequired(false);

        builder.HasIndex(t => new { t.TicketId, t.CreatedAtUtc });
    }
}
