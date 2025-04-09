using Ardalis.Result;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.ProductAggregate;

namespace Rhyme.Net.UseCases.Products.AddRange;

public class AddRangeHandler : ICommandHandler<AddRangeCommand, Result<IEnumerable<Product>>>
{
  private readonly IRepository<Product> _repository;

  public AddRangeHandler(IRepository<Product> repository)
  {
    _repository = repository;
  }

  public async Task<Result<IEnumerable<Product>>> Handle(
    AddRangeCommand request,
    CancellationToken cancellationToken)
  {
    var addedRange = await _repository.AddRangeAsync(request.NewProducts, cancellationToken);

    return Result.Success(addedRange);
  }
}