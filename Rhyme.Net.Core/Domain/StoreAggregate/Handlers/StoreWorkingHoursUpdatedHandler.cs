using Ardalis.GuardClauses;
using Rhyme.Net.Core.Interfaces;
using Rhyme.Net.Core.Domain.StoreAggregate.Events;
using MediatR;

namespace Rhyme.Net.Core.Domain.OrderAggregate.Handlers;

public class StoreWorkingHoursUpdatedHandler : INotificationHandler<StoreWorkingHoursUpdatedEvent>
{
  private readonly IEmailSender _emailSender;

  // In a REAL app you might want to use the Outbox pattern and a command/queue here...
  public StoreWorkingHoursUpdatedHandler(IEmailSender emailSender)
  {
    _emailSender = emailSender;
  }

  // configure a test email server to demo this works
  // https://ardalis.com/configuring-a-local-test-email-server
  public Task Handle(StoreWorkingHoursUpdatedEvent domainEvent, CancellationToken cancellationToken)
  {
    Guard.Against.Null(domainEvent, nameof(domainEvent));

    return _emailSender.SendEmailAsync("test@test.com", "test@test.com", $"{domainEvent.Store.Name} has new working hours.", domainEvent.NewWorkingHours);
  }
}
