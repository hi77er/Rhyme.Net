using Ardalis.Result;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.StoreAggregate;
using Rhyme.Net.UseCases.Stores;
using Rhyme.Net.UseCases.Stores.Update;

namespace Rhyme.Net.UseCases.Stores.Update;

public class UpdateStoreHandler : ICommandHandler<UpdateStoreCommand, Result<StoreDTO>>
{
  private readonly IRepository<Store> _repository;

  public UpdateStoreHandler(IRepository<Store> repository)
  {
    _repository = repository;
  }

  public async Task<Result<StoreDTO>> Handle(UpdateStoreCommand request, CancellationToken cancellationToken)
  {
    var existingEntity = await _repository.GetByIdAsync(request.StoreId, cancellationToken);
    if (existingEntity == null)
    {
      return Result.NotFound();
    }

    existingEntity.UpdateName(request.NewName!);

    await _repository.UpdateAsync(existingEntity, cancellationToken);

    return Result.Success(new StoreDTO(existingEntity.Id, existingEntity.Name));
  }
}
