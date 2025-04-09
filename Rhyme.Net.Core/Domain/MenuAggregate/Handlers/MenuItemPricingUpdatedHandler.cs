using Ardalis.GuardClauses;
using Rhyme.Net.Core.Interfaces;
using Rhyme.Net.Core.Domain.MenuAggregate.Events;
using MediatR;

namespace Rhyme.Net.Core.Domain.MenuAggregate.Handlers;

public class MenuItemPricingUpdatedHandler : INotificationHandler<MenuItemPricingUpdatedEvent>
{
  private readonly IEmailSender _emailSender;

  // In a REAL app you might want to use the Outbox pattern and a command/queue here...
  public MenuItemPricingUpdatedHandler(IEmailSender emailSender)
  {
    _emailSender = emailSender;
  }

  // configure a test email server to demo this works
  // https://ardalis.com/configuring-a-local-test-email-server
  public Task Handle(MenuItemPricingUpdatedEvent domainEvent, CancellationToken cancellationToken)
  {
    Guard.Against.Null(domainEvent.MenuItem.Product, "Product");

    Guard.Against.Null(domainEvent, nameof(domainEvent));
    Task result;

    var oldPrice = domainEvent.OldPricing?.Value ?? 0.0m;
    var newPrice = domainEvent.MenuItem.DefaultPricing?.Value ?? 0.0m;

    if (newPrice < oldPrice)
    {
      result = _emailSender.SendEmailAsync(
        "test@test.com",
        "test@test.com",
        $"{domainEvent.MenuItem.Product.Name} now is cheeper.",
        $"{domainEvent.MenuItem.Product.Name} now costs {newPrice.ToString("C")} instead of {oldPrice.ToString("C")}.");
    }
    else
    {
      result = Task.CompletedTask;
    }

    return result;
  }
}
