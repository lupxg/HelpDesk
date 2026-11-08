using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;

namespace HelpDesk.UnitTests.UnitTests.Domain;

[TestClass]
public class TicketBehaviorTests
{
    private static readonly DateTimeOffset Now = new(2026, 10, 28, 12, 0, 0, TimeSpan.Zero);

    private static readonly Guid SlaPolicyId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    private static Ticket CreateValidTicket(string title = "Cannot access email", string description = "Email access fails.", TicketPriority priority = TicketPriority.Medium)
    {
        return new Ticket(title, description, priority, SlaPolicyId, Now);
    }

    private static Ticket CreateInProgressTicket()
    {
        var ticket = CreateValidTicket();
        ticket.StartProgress();
        return ticket;
    }

    private static Ticket CreateResolvedTicket()
    {
        var ticket = CreateInProgressTicket();
        ticket.Resolve();
        return ticket;
    }

    [TestMethod]
    public void Constructor_WithValidData_CreatesOpenTicket()
    {
        // Act
        var ticket = CreateValidTicket();

        // Assert
        Assert.AreNotEqual(Guid.Empty, ticket.Id);
        Assert.AreEqual("Cannot access email", ticket.Title);
        Assert.AreEqual("Email access fails.", ticket.Description);
        Assert.AreEqual(TicketStatus.Open, ticket.Status);
        Assert.AreEqual(TicketPriority.Medium, ticket.Priority);
        Assert.AreEqual(SlaPolicyId, ticket.SlaPolicyId);
        Assert.AreEqual(Now, ticket.CreatedAtUtc);
        Assert.IsNull(ticket.UpdatedAtUtc);
    }



    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    [TestMethod]
    public void Constructor_WithInvalidTitle_ThrowsArgumentException(string? title)
    {
        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new Ticket(title!, "A valid description.", TicketPriority.Medium, SlaPolicyId, Now));

        // Assert
        Assert.IsNotNull(exception);
        Assert.AreEqual("title", exception.ParamName);
    }



    [DataRow(null)]
    [DataRow("")]
    [DataRow("  ")]
    [TestMethod]
    public void Constructor_WithInvalidDescription_ThrowsArgumentException(string? description)
    {
        // Atc
        var exception = Assert.Throws<ArgumentException>(
            () => new Ticket("Cannot access email", description!, TicketPriority.Medium, SlaPolicyId, Now));

        // Assert
        Assert.IsNotNull(exception);
        Assert.AreEqual("description", exception.ParamName);
    }



    [TestMethod]
    public void Close_WhenTicketIsOpen_ThrowsInvalidOperationException()
    {
        // Arrange
        var ticket = CreateValidTicket();

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => ticket.Close());

        // Assert
        Assert.IsNotNull(exception);
        Assert.Contains("Open", exception.Message);
        Assert.AreEqual(TicketStatus.Open, ticket.Status);
    }



    [TestMethod]
    public void StartProgress_WhenTicketIsOpen_ChangesStatusToInProgress()
    {
        // Arrange
        var ticket = CreateValidTicket();

        // Act
        ticket.StartProgress();

        // Assert
        Assert.AreEqual(TicketStatus.InProgress, ticket.Status);
    }



    [TestMethod]
    public void Constructor_WithInvalidPriority_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var invalidPriority = (TicketPriority)999;

        // Act
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => CreateValidTicket(priority: invalidPriority));

        // Assert
        Assert.AreEqual("priority", exception.ParamName);
    }

    [TestMethod]
    public void Resolve_WhenTicketIsInProgress_ChangesStatusToResolved()
    {
        // Arrange
        var ticket = CreateValidTicket();
        ticket.StartProgress();

        // Act
        ticket.Resolve();

        // Assert
        Assert.AreEqual(TicketStatus.Resolved, ticket.Status);
    }

    [TestMethod]
    public void Close_WhenTicketIsResolved_ChangesStatusToClosed()
    {
        // Arrange
        var ticket = CreateValidTicket();
        ticket.StartProgress();
        ticket.Resolve();

        // Act
        ticket.Close();

        // Assert
        Assert.AreEqual(TicketStatus.Closed, ticket.Status);
    }

    [TestMethod]
    public void AssignToTeam_WithValidTeamId_AssignsTeam()
    {
        // Arrange
        var ticket = CreateValidTicket();
        var teamId = Guid.NewGuid();

        // Act
        ticket.AssignToTeam(teamId);

        // Assert
        Assert.AreEqual(teamId, ticket.TeamId);
    }

    [TestMethod]
    public void AssignToTeam_WithEmptyGuid_ThrowsArgumentException()
    {
        // Arrange
        var ticket = CreateValidTicket();

        // Act
        var exception = Assert.Throws<ArgumentException>(() => ticket.AssignToTeam(Guid.Empty));

        // Assert
        Assert.AreEqual("teamId", exception.ParamName);
        Assert.IsNull(ticket.TeamId);
    }

    [TestMethod]
    public void AssignToTeam_WithSameTeam_DoesNotChangeAssignment()
    {
        // Arrange
        var ticket = CreateValidTicket();
        var teamId = Guid.NewGuid();
        ticket.AssignToTeam(teamId);

        // Act
        ticket.AssignToTeam(teamId);

        // Assert
        Assert.AreEqual(teamId, ticket.TeamId);
    }

    [TestMethod]
    public void AssignToTeam_WithDifferentTeam_ReassignsTicket()
    {
        // Arrange
        var ticket = CreateValidTicket();
        var originalTeamId = Guid.NewGuid();
        var newTeamId = Guid.NewGuid();

        ticket.AssignToTeam(originalTeamId);

        // Act
        ticket.AssignToTeam(newTeamId);

        // Assert
        Assert.AreEqual(newTeamId, ticket.TeamId);
    }


    #region Unassignment

    [TestMethod]
    public void UnassignUser_WhenUserIsAssigned_ClearsAssignment()
    {
        // Arrange
        var ticket = CreateValidTicket();
        ticket.AssignTo(Guid.NewGuid());

        // Act
        ticket.UnassignUser();

        // Assert
        Assert.IsNull(ticket.AssignedToId);
    }

    [TestMethod]
    public void UnassignUser_WhenNoUserIsAssigned_RemainsUnassigned()
    {
        // Arrange
        var ticket = CreateValidTicket();

        // Act
        ticket.UnassignUser();

        // Assert
        Assert.IsNull(ticket.AssignedToId);
    }

    [TestMethod]
    public void UnassignTeam_WhenTeamIsAssigned_ClearsAssignment()
    {
        // Arrange
        var ticket = CreateValidTicket();
        ticket.AssignToTeam(Guid.NewGuid());

        // Act
        ticket.UnassignTeam();

        // Assert
        Assert.IsNull(ticket.TeamId);
    }

    [TestMethod]
    public void UnassignTeam_WhenNoTeamIsAssigned_RemainsUnassigned()
    {
        // Arrange
        var ticket = CreateValidTicket();

        // Act
        ticket.UnassignTeam();

        // Assert
        Assert.IsNull(ticket.TeamId);
    }

    #endregion

    #region ChangePriority

    [DataRow(TicketPriority.Low)]
    [DataRow(TicketPriority.Medium)]
    [DataRow(TicketPriority.High)]
    [DataRow(TicketPriority.Critical)]
    [TestMethod]
    public void ChangePriority_WithValidPriority_UpdatesPriority(TicketPriority priority)
    {
        // Arrange
        var ticket = CreateValidTicket();

        // Act
        ticket.ChangePriority(priority);

        // Assert
        Assert.AreEqual(priority, ticket.Priority);
    }

    [TestMethod]
    public void ChangePriority_WithInvalidPriority_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var ticket = CreateValidTicket();
        var originalPriority = ticket.Priority;

        // Act
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => ticket.ChangePriority((TicketPriority)999));

        // Assert
        Assert.AreEqual("priority", exception.ParamName);
        Assert.AreEqual(originalPriority, ticket.Priority);
    }

    [TestMethod]
    public void ChangePriority_WithSamePriority_LeavesPriorityUnchanged()
    {
        // Arrange
        var ticket = CreateValidTicket(priority: TicketPriority.High);

        // Act
        ticket.ChangePriority(TicketPriority.High);

        // Assert
        Assert.AreEqual(TicketPriority.High, ticket.Priority);
    }

    #endregion

    #region SetDueAt

    [TestMethod]
    public void SetDueAt_WithDate_SetsDueDate()
    {
        // Arrange
        var ticket = CreateValidTicket();
        var dueAt = Now.AddDays(2);

        // Act
        ticket.SetDueAt(dueAt);

        // Assert
        Assert.AreEqual(dueAt, ticket.DueAt);
    }

    [TestMethod]
    public void SetDueAt_WithNull_ClearsDueDate()
    {
        // Arrange
        var ticket = CreateValidTicket();
        ticket.SetDueAt(Now.AddDays(2));

        // Act
        ticket.SetDueAt(null);

        // Assert
        Assert.IsNull(ticket.DueAt);
    }

    #endregion

    #region SetNumber

    [TestMethod]
    public void SetNumber_WithPositiveNumber_SetsNumber()
    {
        // Arrange
        var ticket = CreateValidTicket();
        const long number = 1001;

        // Act
        ticket.SetNumber(number);

        // Assert
        Assert.AreEqual(number, ticket.Number);
    }


    [DataRow(0)]
    [DataRow(-1)]
    [TestMethod]
    public void SetNumber_WithNonPositiveNumber_ThrowsArgumentOutOfRangeException(long number)
    {
        // Arrange
        var ticket = CreateValidTicket();

        // Act
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => ticket.SetNumber(number));

        // Assert
        Assert.AreEqual("number", exception.ParamName);
        Assert.AreEqual(0, ticket.Number);
    }

    [TestMethod]
    public void SetNumber_WhenNumberAlreadyAssigned_ThrowsInvalidOperationException()
    {
        // Arrange
        var ticket = CreateValidTicket();
        ticket.SetNumber(1001);

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => ticket.SetNumber(1002));

        // Assert
        Assert.AreEqual("The ticket number has already been assigned.", exception.Message);

        Assert.AreEqual(1001, ticket.Number);
    }

    #endregion

    #region StartProgress

    [TestMethod]
    public void StartProgress_WhenOpen_ChangesStatusToInProgress()
    {
        // Arrange
        var ticket = CreateValidTicket();

        // Act
        ticket.StartProgress();

        // Assert
        Assert.AreEqual(TicketStatus.InProgress, ticket.Status);
    }

    [TestMethod]
    public void StartProgress_WhenPending_ChangesStatusToInProgress()
    {
        // Arrange
        var ticket = CreateInProgressTicket();
        ticket.PutOnHold();

        // Act
        ticket.StartProgress();

        // Assert
        Assert.AreEqual(TicketStatus.InProgress, ticket.Status);
    }

    [TestMethod]
    public void StartProgress_WhenAlreadyInProgress_ThrowsInvalidOperationException()
    {
        // Arrange
        var ticket = CreateInProgressTicket();

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => ticket.StartProgress());

        // Assert
        Assert.Contains("InProgress", exception.Message);
        Assert.AreEqual(TicketStatus.InProgress, ticket.Status);
    }

    #endregion

    #region PutOnHold

    [TestMethod]
    public void PutOnHold_WhenInProgress_ChangesStatusToPending()
    {
        // Arrange
        var ticket = CreateInProgressTicket();

        // Act
        ticket.PutOnHold();

        // Assert
        Assert.AreEqual(TicketStatus.Pending, ticket.Status);
    }

    [TestMethod]
    public void PutOnHold_WhenOpen_ThrowsInvalidOperationException()
    {
        // Arrange
        var ticket = CreateValidTicket();

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => ticket.PutOnHold());

        // Assert
        Assert.Contains("Open", exception.Message);
        Assert.AreEqual(TicketStatus.Open, ticket.Status);
    }

    [TestMethod]
    public void PutOnHold_WhenPending_ThrowsInvalidOperationException()
    {
        // Arrange
        var ticket = CreateInProgressTicket();
        ticket.PutOnHold();

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => ticket.PutOnHold());

        // Assert
        Assert.Contains("Pending", exception.Message);
        Assert.AreEqual(TicketStatus.Pending, ticket.Status);
    }

    #endregion

    #region Resolve

    [TestMethod]
    public void Resolve_WhenInProgress_ChangesStatusToResolved()
    {
        // Arrange
        var ticket = CreateInProgressTicket();

        // Act
        ticket.Resolve();

        // Assert
        Assert.AreEqual(TicketStatus.Resolved, ticket.Status);
    }

    [TestMethod]
    public void Resolve_WhenPending_ChangesStatusToResolved()
    {
        // Arrange
        var ticket = CreateInProgressTicket();
        ticket.PutOnHold();

        // Act
        ticket.Resolve();

        // Assert
        Assert.AreEqual(TicketStatus.Resolved, ticket.Status);
    }

    [TestMethod]
    public void Resolve_WhenOpen_ThrowsInvalidOperationException()
    {
        // Arrange
        var ticket = CreateValidTicket();

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => ticket.Resolve());

        // Assert
        Assert.Contains("Open", exception.Message);
        Assert.AreEqual(TicketStatus.Open, ticket.Status);
    }

    #endregion

    #region Close

    [TestMethod]
    public void Close_WhenResolved_ChangesStatusToClosed()
    {
        // Arrange
        var ticket = CreateResolvedTicket();

        // Act
        ticket.Close();

        // Assert
        Assert.AreEqual(TicketStatus.Closed, ticket.Status);
    }

    [TestMethod]
    public void Close_WhenInProgress_ThrowsInvalidOperationException()
    {
        // Arrange
        var ticket = CreateInProgressTicket();

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => ticket.Close());

        // Assert
        Assert.Contains("InProgress", exception.Message);
        Assert.AreEqual(TicketStatus.InProgress, ticket.Status);
    }

    [TestMethod]
    public void Close_WhenAlreadyClosed_ThrowsInvalidOperationException()
    {
        // Arrange
        var ticket = CreateResolvedTicket();
        ticket.Close();

        // Act
        var exception = Assert.Throws<InvalidOperationException>(
            () => ticket.Close());

        // Assert
        Assert.Contains("Closed", exception.Message);
        Assert.AreEqual(TicketStatus.Closed, ticket.Status);
    }

    #endregion

    #region Reopen

    [TestMethod]
    public void Reopen_WhenResolved_ChangesStatusToInProgress()
    {
        // Arrange
        var ticket = CreateResolvedTicket();

        // Act
        ticket.Reopen();

        // Assert
        Assert.AreEqual(TicketStatus.InProgress, ticket.Status);
    }

    [TestMethod]
    public void Reopen_WhenOpen_ThrowsInvalidOperationException()
    {
        // Arrange
        var ticket = CreateValidTicket();

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => ticket.Reopen());

        // Assert
        Assert.Contains("Open", exception.Message);
        Assert.AreEqual(TicketStatus.Open, ticket.Status);
    }

    [TestMethod]
    public void Reopen_WhenClosed_ThrowsInvalidOperationException()
    {
        // Arrange
        var ticket = CreateResolvedTicket();
        ticket.Close();

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => ticket.Reopen());

        // Assert
        Assert.Contains("Closed", exception.Message);
        Assert.AreEqual(TicketStatus.Closed, ticket.Status);
    }

    #endregion
}
