using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Rhyme.Net.UseCases.Products.ListShallow;

public record ListProductsShallowQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<ProductDTO>>>;
