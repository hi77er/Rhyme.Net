using Ardalis.Result;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.MenuAggregate;
using Rhyme.Net.Core.Domain.MenuAggregate.Specifications;

namespace Rhyme.Net.UseCases.Menus.GetForAdminPanel;

/// <summary>
/// Queries don't necessarily need to use repository methods, but they can if it's convenient
/// </summary>
public class GetMenuForAdminPanelHandler : IQueryHandler<GetMenuForAdminPanelQuery, Result<MenuForAdminPanelDTO>>
{
  private readonly IReadRepository<Menu> _repository;

  public GetMenuForAdminPanelHandler(IReadRepository<Menu> repository)
  {
    _repository = repository;
  }

  public async Task<Result<MenuForAdminPanelDTO>> Handle(GetMenuForAdminPanelQuery request, CancellationToken cancellationToken)
  {
    var spec = new MenuForAdminPanelSpec(request.StoreId);
    var entity = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (entity == null) return Result.NotFound();

    var brochures = entity
      .Brochures
      .Select(i => new BrochureDTO(i.Id, i.Title, i.Description))
      .ToList();

    return new MenuForAdminPanelDTO(entity.Id, entity.Title, brochures);
  }
}