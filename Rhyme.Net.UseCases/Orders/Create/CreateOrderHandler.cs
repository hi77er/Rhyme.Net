using Ardalis.Result;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.OrderAggregate;

namespace Rhyme.Net.UseCases.Orders.Create;

public class CreateOrderHandler : ICommandHandler<CreateOrderCommand, Result<Guid>>
{
  private readonly IRepository<Order> _repository;

  public CreateOrderHandler(IRepository<Order> repository)
  {
    _repository = repository;
  }

  public async Task<Result<Guid>> Handle(CreateOrderCommand request,
    CancellationToken cancellationToken)
  {
    var newOrder = new Order(request.StoreId);
    var createdItem = await _repository.AddAsync(newOrder, cancellationToken);

    return Result.Success(createdItem.Id);
  }
}