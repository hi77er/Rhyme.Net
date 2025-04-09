using Ardalis.Result;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.MenuAggregate;

namespace Rhyme.Net.UseCases.Menus.Delete;

public class DeleteMenuHandler : ICommandHandler<DeleteMenuCommand, Result>
{
  private readonly IRepository<Menu> _repository;

  public DeleteMenuHandler(IRepository<Menu> repository)
  {
    _repository = repository;
  }

  public async Task<Result> Handle(DeleteMenuCommand request, CancellationToken cancellationToken)
  {
    var aggregateToDelete = await _repository.GetByIdAsync(request.MenuId, cancellationToken);
    if (aggregateToDelete == null)
    {
      return Result.NotFound();
    }

    await _repository.DeleteAsync(aggregateToDelete, cancellationToken);

    return Result.Success();
  }
}
