using Microsoft.EntityFrameworkCore;
using Rhyme.Net.UseCases.Products.ListShallow;
using Rhyme.Net.UseCases.Products;

namespace Rhyme.Net.Infrastructure.Data.EF.Queries;

public class ListProductsShallowQueryService(AppDbContext db) : IListProductsShallowQueryService
{
  private readonly AppDbContext _db = db;

  public async Task<IEnumerable<ProductDTO>> ListAsync()
  {
    var result = await _db.Products.FromSqlRaw("SELECT Id, Name FROM Products") // don't fetch other big columns
      .Select(x => new ProductDTO(x.Id, x.Name))
      .ToListAsync();

    return result;
  }
}
