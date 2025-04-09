using Ardalis.GuardClauses;
using Rhyme.Net.Core.Interfaces;
using Rhyme.Net.Core.Domain.OrderAggregate.Events;
using MediatR;

namespace Rhyme.Net.Core.Domain.OrderAggregate.Handlers;

public class OrderSubmittedInventoryUpdateHandler : INotificationHandler<OrderSubmittedEvent>
{
  // private readonly IRepository<InventoryItem> _repository;

  // public OrderSubmittedEmailNotificationHandler(IRepository<Project> repository)
  // {
  //   _repository = repository;
  // }

  public Task Handle(OrderSubmittedEvent domainEvent, CancellationToken cancellationToken)
  {
    Guard.Against.Null(domainEvent, nameof(domainEvent));

    // _repository.UpdateAsync(domainEvent.Order.Items.Select(i => new InventoryItem(i.ProductId, i.Quantity)), cancellationToken);
    return Task.CompletedTask;
  }
}
