using Ardalis.GuardClauses;
using Rhyme.Net.Core.Interfaces;
using Rhyme.Net.Core.Domain.OrderAggregate.Events;
using MediatR;

namespace Rhyme.Net.Core.Domain.OrderAggregate.Handlers;

public class OrderSubmittedEmailNotificationHandler : INotificationHandler<OrderSubmittedEvent>
{
  private readonly IEmailSender _emailSender;

  // In a REAL app you might want to use the Outbox pattern and a command/queue here...
  public OrderSubmittedEmailNotificationHandler(IEmailSender emailSender)
  {
    _emailSender = emailSender;
  }

  // configure a test email server to demo this works
  // https://ardalis.com/configuring-a-local-test-email-server
  public Task Handle(OrderSubmittedEvent domainEvent, CancellationToken cancellationToken)
  {
    Guard.Against.Null(domainEvent, nameof(domainEvent));

    var sender = "sender@test.com";
    var receiver = "receiver@test.com";
    var emailSubject = $"Order submitted";
    var emailBody = $"Your order #{domainEvent.Order.Id} has been submitted. Order details: {domainEvent.Order.ToString()}";

    return _emailSender.SendEmailAsync(sender, receiver, emailSubject, emailBody);
  }
}
