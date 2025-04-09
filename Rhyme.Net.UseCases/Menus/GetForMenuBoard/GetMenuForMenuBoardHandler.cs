using Ardalis.Result;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.MenuAggregate;
using Rhyme.Net.Core.Domain.MenuAggregate.Specifications;

namespace Rhyme.Net.UseCases.Menus.GetForMenuBoard;

/// <summary>
/// Queries don't necessarily need to use repository methods, but they can if it's convenient
/// </summary>
public class GetMenuForMenuBoardHandler : IQueryHandler<GetMenuForMenuBoardQuery, Result<MenuForMenuBoardDTO>>
{
  private readonly IReadRepository<Menu> _repository;

  public GetMenuForMenuBoardHandler(IReadRepository<Menu> repository)
  {
    _repository = repository;
  }

  public async Task<Result<MenuForMenuBoardDTO>> Handle(GetMenuForMenuBoardQuery request, CancellationToken cancellationToken)
  {
    var spec = new MenuForMenuBoardSpec(request.StoreId);
    var entity = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (entity == null) return Result.NotFound();

    var brochures = entity
      .Brochures
      .Select(i => new BrochureDTO(i.Id, i.Title, i.Description))
      .ToList();

    return new MenuForMenuBoardDTO(entity.Id, entity.Title, brochures);
  }
}