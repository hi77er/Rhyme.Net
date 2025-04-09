using Ardalis.Result;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.MenuAggregate;
using Rhyme.Net.Core.Domain.MenuAggregate.Events;
using Rhyme.Net.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Ardalis.GuardClauses;

namespace Rhyme.Net.Core.Services;

/// <summary>
/// This is here mainly so there's an example of a domain service
/// and also to demonstrate how to fire domain events from a service.
/// </summary>
/// <param name="_repository"></param>
/// <param name="_mediator"></param>
/// <param name="_logger"></param>
public class DeleteMenuService(
  IRepository<Menu> _repository,
  IMediator _mediator,
  ILogger<DeleteMenuService> _logger) : IDeleteMenuService
{
  // NOTE: 
  // This is a Domain Service.
  // Domain services are used to encapsulate domain logic that doesn't naturally fit within an entity or value object.
  // This often involves logic that spans multiple aggregates or requires external dependencies. 

  public async Task<Result> DeleteMenu(Guid menuId)
  {
    Guard.Against.Default(menuId, nameof(menuId));
    _logger.LogInformation("Deleting Menu {menuId}", menuId.ToString());

    Menu? aggregateToDelete = await _repository.GetByIdAsync(menuId);

    if (aggregateToDelete == null) return Result.NotFound();
    await _repository.DeleteAsync(aggregateToDelete);

    var domainEvent = new MenuDeletedEvent(aggregateToDelete);
    await _mediator.Publish(domainEvent);

    return Result.Success();
  }
}
