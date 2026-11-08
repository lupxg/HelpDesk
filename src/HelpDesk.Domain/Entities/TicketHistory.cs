using HelpDesk.Domain.Common;
using HelpDesk.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Domain.Entities;

public class TicketHistory : BaseEntity<Guid>
{
    public Guid? TicketId { get; private set; }
    public Guid? ActorId { get; private set; }
    public TicketEventType EventType { get; private set; }
    public string? OldValue { get; private set; } = string.Empty;
    public string? NewValue { get; private set; } = string.Empty;

    private TicketHistory() {}

    public TicketHistory(Guid ticketId, Guid? actorId, TicketEventType eventType, string? oldValue = null, string? newValue = null) : base(Guid.NewGuid())
    {
        if (ticketId == Guid.Empty)
            throw new ArgumentException("Ticket ID cannot be empty.", nameof(ticketId));

        if (actorId == Guid.Empty)
            throw new ArgumentException("Actor ID cannot be an empty GUID.", nameof(actorId));

        if (!Enum.IsDefined(eventType))
            throw new ArgumentException("Event type cannot be empty.", nameof(eventType));

        TicketId = ticketId;
        ActorId = actorId;
        EventType = eventType;
        OldValue = oldValue;
        NewValue = newValue;
    }
}
