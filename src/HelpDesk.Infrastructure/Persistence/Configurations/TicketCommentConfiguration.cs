using HelpDesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelpDesk.Infrastructure.Persistence.Configurations;

public sealed class TicketCommentConfiguration : IEntityTypeConfiguration<TicketComment>
{
    public void Configure(EntityTypeBuilder<TicketComment> builder)
    {
        builder.ToTable("TicketComments");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.TicketId).IsRequired();

        builder.Property(t => t.AuthorId).IsRequired();

        builder.Property(t => t.Body).IsRequired().HasMaxLength(10_000);

        builder.Property(t => t.CreatedAtUtc).IsRequired();

        builder.Property(t => t.UpdatedAtUtc).IsRequired(false);

        builder.HasIndex(t => t.TicketId);
    }
}
