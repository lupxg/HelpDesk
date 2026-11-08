using HelpDesk.Domain.Entities;
using HelpDesk.Infrastructure.Authentication.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelpDesk.Infrastructure.Persistence.Configurations;

public sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Number).IsRequired();

        builder.HasIndex(t => t.Number).IsUnique();

        builder.Property(t => t.Title).IsRequired().HasMaxLength(200);

        builder.Property(t => t.Description).IsRequired().HasMaxLength(10_000);

        builder.Property(t => t.Status).IsRequired().HasConversion<string>().HasMaxLength(30);

        builder.Property(t => t.Priority).IsRequired().HasConversion<string>().HasMaxLength(30);

        builder.Property(t => t.CreatedById).IsRequired(false);

        builder.Property(t => t.AssignedToId).IsRequired(false);

        builder.Property(t => t.TeamId).IsRequired(false);

        builder.Property(t => t.DueAt).IsRequired(false);

        builder.Property(t => t.SlaPolicyId).IsRequired();

        builder.Property(t => t.CreatedAtUtc).IsRequired();

        builder.Property(t => t.UpdatedAtUtc).IsRequired(false);

        builder.HasOne<ApplicationUser>().WithMany().HasForeignKey(t => t.CreatedById).IsRequired(false).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ApplicationUser>().WithMany().HasForeignKey(t => t.AssignedToId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<SupportTeam>().WithMany().HasForeignKey(t => t.TeamId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<SlaPolicy>().WithMany().HasForeignKey(t => t.SlaPolicyId).IsRequired().OnDelete(DeleteBehavior.Restrict);
    }
}
