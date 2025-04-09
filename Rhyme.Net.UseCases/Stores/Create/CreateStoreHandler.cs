using Ardalis.Result;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.StoreAggregate;

namespace Rhyme.Net.UseCases.Stores.Create;

public class CreateStoreHandler : ICommandHandler<CreateStoreCommand, Result<Guid>>
{
  private readonly IRepository<Store> _repository;

  public CreateStoreHandler(IRepository<Store> repository)
  {
    _repository = repository;
  }

  public async Task<Result<Guid>> Handle(CreateStoreCommand request,
    CancellationToken cancellationToken)
  {
    var newStore = new Store(request.Name);
    var createdItem = await _repository.AddAsync(newStore, cancellationToken);

    return createdItem.Id;
  }
}