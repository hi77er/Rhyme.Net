using Microsoft.EntityFrameworkCore;
using Rhyme.Net.UseCases.Stores.ListShallow;
using Rhyme.Net.UseCases.Stores;

namespace Rhyme.Net.Infrastructure.Data.EF.Queries;

public class ListStoresShallowQueryService(AppDbContext db) : IListStoresShallowQueryService
{
  private readonly AppDbContext _db = db;

  public async Task<IEnumerable<StoreDTO>> ListAsync()
  {
    var result = await _db.Stores.FromSqlRaw("SELECT Id, Name FROM Stores") // don't fetch other big columns
      .Select(x => new StoreDTO(x.Id, x.Name))
      .ToListAsync();

    return result;
  }
}