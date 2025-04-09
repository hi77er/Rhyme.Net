using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Interfaces;
using Rhyme.Net.Core.Domain.MenuAggregate;
using Rhyme.Net.Core.Domain.StoreAggregate.Events;
using MediatR;

namespace Rhyme.Net.Core.Domain.OrderAggregate.Handlers;

public class StoreMenuUpdatedHandler : INotificationHandler<StoreMenuUpdatedEvent>
{
  private readonly IEmailSender _emailSender;
  private readonly IRepository<Menu> _repository;

  // In a REAL app you might want to use the Outbox pattern and a command/queue here...
  public StoreMenuUpdatedHandler(IEmailSender emailSender, IRepository<Menu> repository)
  {
    _emailSender = emailSender;
    _repository = repository;
  }

  // configure a test email server to demo this works
  // https://ardalis.com/configuring-a-local-test-email-server
  public Task Handle(StoreMenuUpdatedEvent domainEvent, CancellationToken cancellationToken)
  {
    Guard.Against.Null(domainEvent, nameof(domainEvent));

    Menu newMenu;
    if (domainEvent.Store.MenuId.HasValue)
    {
      // If the store has a menu ID, we can proceed to fetch the menu details.
      // If not, we can skip sending the email or handle it differently.
      var newMenuTask = _repository.GetByIdAsync(domainEvent.Store.MenuId.Value, cancellationToken);
      if (newMenuTask.Result == null)
        throw new Exception($"Menu with ID {domainEvent.Store.MenuId} not found.");
      newMenu = newMenuTask.Result;
    }
    else
    {
      throw new Exception($"Store with ID {domainEvent.Store.Id} does not have a MenuId.");
    }

    var sender = "sender@test.com";
    var receiver = "receiver@test.com";
    var emailSubject = $"Store has a new Menu.";
    var emailBody = $"{domainEvent.Store.Name} has a new Menu. Menu details: {newMenu.ToString()}";

    return _emailSender.SendEmailAsync(sender, receiver, emailSubject, emailBody);
  }
}
