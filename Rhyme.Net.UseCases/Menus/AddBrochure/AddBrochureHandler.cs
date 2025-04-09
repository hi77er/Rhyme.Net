using Ardalis.Result;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.MenuAggregate;
using Rhyme.Net.Core.Domain.MenuAggregate.Specifications;

namespace Rhyme.Net.UseCases.Menus.AddBrochure;

public class AddBrochureHandler : ICommandHandler<AddBrochureCommand, Result<Guid>>
{
  private readonly IRepository<Menu> _repository;

  public AddBrochureHandler(IRepository<Menu> repository)
  {
    _repository = repository;
  }

  public async Task<Result<Guid>> Handle(AddBrochureCommand request, CancellationToken cancellationToken)
  {
    var spec = new MenuForAdminPanelSpec(request.StoreId);
    var menu = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (menu == null) return Result.NotFound();

    var newBrochure = new Brochure(request.MenuId, request.Title);

    menu.AddBrochure(newBrochure);
    await _repository.UpdateAsync(menu);

    return Result.Success(newBrochure.Id);
  }
}