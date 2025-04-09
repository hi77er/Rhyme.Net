using Ardalis.Result;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.MenuAggregate;
using Rhyme.Net.Core.Domain.MenuAggregate.Specifications;

namespace Rhyme.Net.UseCases.Menus.GetForPos;

/// <summary>
/// Queries don't necessarily need to use repository methods, but they can if it's convenient
/// </summary>
public class GetMenuForPosHandler : IQueryHandler<GetMenuForPosQuery, Result<MenuForPosDTO>>
{
  private readonly IReadRepository<Menu> _repository;

  public GetMenuForPosHandler(IReadRepository<Menu> repository)
  {
    _repository = repository;
  }

  public async Task<Result<MenuForPosDTO>> Handle(GetMenuForPosQuery request, CancellationToken cancellationToken)
  {
    var spec = new MenuForPosSpec(request.StoreId);
    var entity = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (entity == null) return Result.NotFound();

    var brochures = entity
      .Brochures
      .Select(i => new BrochureDTO(i.Id, i.Title, i.Description))
      .ToList();

    return new MenuForPosDTO(entity.Id, entity.Title, brochures);
  }
}