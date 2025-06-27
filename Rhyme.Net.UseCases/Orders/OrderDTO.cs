namespace Rhyme.Net.UseCases.Orders;

public record OrderDTO(Guid Id, Guid StoreId, decimal Total);
