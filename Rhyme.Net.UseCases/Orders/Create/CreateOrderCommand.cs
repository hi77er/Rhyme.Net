using Ardalis.Result;

namespace Rhyme.Net.UseCases.Orders.Create;

/// <summary>
/// Create a new Menu.
/// </summary>
/// <param name="OrderId"></param>
/// <param name="StoreId"></param>
public record CreateOrderCommand(Guid StoreId) : Ardalis.SharedKernel.ICommand<Result<Guid>>;