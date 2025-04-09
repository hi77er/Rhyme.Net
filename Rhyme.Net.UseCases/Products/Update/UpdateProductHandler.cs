using Ardalis.Result;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.ProductAggregate;
using Rhyme.Net.UseCases.Products;
using Rhyme.Net.UseCases.Products.Update;

namespace Rhyme.Net.UseCases.Products.Update;

public class UpdateProductHandler : ICommandHandler<UpdateProductCommand, Result<ProductDTO>>
{
  private readonly IRepository<Product> _repository;

  public UpdateProductHandler(IRepository<Product> repository)
  {
    _repository = repository;
  }

  public async Task<Result<ProductDTO>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
  {
    var existingEntity = await _repository.GetByIdAsync(request.ProductId, cancellationToken);
    if (existingEntity == null)
    {
      return Result.NotFound();
    }

    existingEntity.UpdateName(request.NewName!);

    await _repository.UpdateAsync(existingEntity, cancellationToken);

    return Result.Success(new ProductDTO(existingEntity.Id, existingEntity.Name));
  }
}
