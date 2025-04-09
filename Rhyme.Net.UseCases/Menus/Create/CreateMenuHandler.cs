using Ardalis.Result;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.MenuAggregate;

namespace Rhyme.Net.UseCases.Menus.Create;

public class CreateMenuHandler : ICommandHandler<CreateMenuCommand, Result<Guid>>
{
  private readonly IRepository<Menu> _repository;

  public CreateMenuHandler(IRepository<Menu> repository)
  {
    _repository = repository;
  }

  public async Task<Result<Guid>> Handle(CreateMenuCommand request,
    CancellationToken cancellationToken)
  {
    var newMenu = new Menu(request.StoreId, request.Title);
    var createdItem = await _repository.AddAsync(newMenu, cancellationToken);

    return Result.Success(createdItem.Id);
  }
}