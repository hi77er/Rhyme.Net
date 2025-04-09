using Ardalis.Result;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.ProductAggregate;

namespace Rhyme.Net.UseCases.Products.Create;

public class CreateProductHandler : ICommandHandler<CreateProductCommand, Result<Guid>>
{
  private readonly IRepository<Product> _repository;

  public CreateProductHandler(IRepository<Product> repository)
  {
    _repository = repository;
  }

  public async Task<Result<Guid>> Handle(CreateProductCommand request,
    CancellationToken cancellationToken)
  {
    var newProduct = new Product(request.Name);
    var createdItem = await _repository.AddAsync(newProduct, cancellationToken);

    return Result.Success(createdItem.Id);
  }
}