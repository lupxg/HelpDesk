using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;
using HelpDesk.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.UnitTests.UnitTest.Infrastructure;

[TestClass]
public class AuditingInterceptorTests
{

    private static TestDbContext CreateContext(FixedTimeProvider clock)
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .AddInterceptors(new AuditingInterceptor(clock))
            .Options;

        return new TestDbContext(options);
    }

    private static Ticket CreateTicket(DateTimeOffset now)
    {
        return new Ticket("Cannot access email", "Email access fails.", TicketPriority.Medium, Guid.NewGuid(), now);
    }


    [TestMethod]
    public async Task SaveChangesAsync_WhenTicketIsAdded_SetsCreationTimestamp()
    {
        // Arrange
        var clock = new FixedTimeProvider(new DateTimeOffset(2026, 10, 28, 12, 0, 0, TimeSpan.Zero));

        await using var context = CreateContext(clock);

        var ticket = CreateTicket(clock.GetUtcNow());
        context.Tickets.Add(ticket);

        // Act
        await context.SaveChangesAsync();

        // Assert
        Assert.AreEqual(clock.GetUtcNow(), ticket.CreatedAtUtc);
        Assert.IsNull(ticket.UpdatedAtUtc);
    }

    [TestMethod]
    public async Task SaveChangesAsync_WhenTicketIsModified_UpdatesTimestamp()
    {
        // Arrange
        var createdAt = new DateTimeOffset(2026, 10, 28, 12, 0, 0, TimeSpan.Zero);

        var updatedAt = createdAt.AddHours(1);

        var clock = new FixedTimeProvider(createdAt);

        await using var context = CreateContext(clock);

        var ticket = CreateTicket(createdAt);
        context.Tickets.Add(ticket);

        await context.SaveChangesAsync();

        // Act
        clock.SetUtcNow(updatedAt);

        ticket.ChangePriority(TicketPriority.High);

        await context.SaveChangesAsync();

        // Assert
        Assert.AreEqual(createdAt, ticket.CreatedAtUtc);
        Assert.AreEqual(updatedAt, ticket.UpdatedAtUtc);
        Assert.AreEqual(TicketPriority.High, ticket.Priority);
    }

    private sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        public DbSet<Ticket> Tickets => Set<Ticket>();
    }

    private sealed class FixedTimeProvider(DateTimeOffset initialTime) : TimeProvider
    {
        private DateTimeOffset _now = initialTime;

        public override DateTimeOffset GetUtcNow() => _now;

        public void SetUtcNow(DateTimeOffset now) => _now = now;
    }
}
