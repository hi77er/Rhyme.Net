using Ardalis.Result;

namespace Rhyme.Net.Core.Interfaces;

public interface IDeleteMenuService
{
  // This service and method exist to provide a place in which to fire domain events
  // when deleting this aggregate root entity
  public Task<Result> DeleteMenu(Guid menuId);
}
