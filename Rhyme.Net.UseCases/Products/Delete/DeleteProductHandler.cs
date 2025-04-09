using Ardalis.Result;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.ProductAggregate;

namespace Rhyme.Net.UseCases.Products.Delete;

public class DeleteProductHandler : ICommandHandler<DeleteProductCommand, Result>
{
  private readonly IRepository<Product> _repository;

  public DeleteProductHandler(IRepository<Product> repository)
  {
    _repository = repository;
  }

  public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
  {
    var aggregateToDelete = await _repository.GetByIdAsync(request.ProductId, cancellationToken);
    if (aggregateToDelete == null)
    {
      return Result.NotFound();
    }

    await _repository.DeleteAsync(aggregateToDelete, cancellationToken);

    return Result.Success();
  }
}
