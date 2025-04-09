using Ardalis.Result;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.StoreAggregate;

namespace Rhyme.Net.UseCases.Stores.Delete;

public class DeleteStoreHandler : ICommandHandler<DeleteStoreCommand, Result>
{
  private readonly IRepository<Store> _repository;

  public DeleteStoreHandler(IRepository<Store> repository)
  {
    _repository = repository;
  }

  public async Task<Result> Handle(DeleteStoreCommand request, CancellationToken cancellationToken)
  {
    var aggregateToDelete = await _repository.GetByIdAsync(request.StoreId, cancellationToken);
    if (aggregateToDelete == null)
    {
      return Result.NotFound();
    }

    await _repository.DeleteAsync(aggregateToDelete, cancellationToken);

    return Result.Success();
  }
}
