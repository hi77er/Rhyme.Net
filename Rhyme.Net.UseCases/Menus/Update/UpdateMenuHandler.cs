using Ardalis.Result;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.MenuAggregate;
using Rhyme.Net.UseCases.Menus;
using Rhyme.Net.UseCases.Menus.Update;

namespace Rhyme.Net.UseCases.Menus.Update;

public class UpdateMenuHandler : ICommandHandler<UpdateMenuCommand, Result<MenuDTO>>
{
  private readonly IRepository<Menu> _repository;

  public UpdateMenuHandler(IRepository<Menu> repository)
  {
    _repository = repository;
  }

  public async Task<Result<MenuDTO>> Handle(UpdateMenuCommand request, CancellationToken cancellationToken)
  {
    var existingEntity = await _repository.GetByIdAsync(request.MenuId, cancellationToken);
    if (existingEntity == null)
    {
      return Result.NotFound();
    }

    existingEntity.UpdateTitle(request.NewTitle!);

    await _repository.UpdateAsync(existingEntity, cancellationToken);

    return Result.Success(new MenuDTO(existingEntity.Id, existingEntity.Title));
  }
}
