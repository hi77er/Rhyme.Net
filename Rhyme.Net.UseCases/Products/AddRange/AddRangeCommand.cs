using Ardalis.Result;
using Rhyme.Net.Core.Domain.ProductAggregate;

namespace Rhyme.Net.UseCases.Products.AddRange;

/// <summary>
/// Add a range of new products.
/// </summary>
/// <param name="NewProducts"></param>
public record AddRangeCommand(IEnumerable<Product> NewProducts) : Ardalis.SharedKernel.ICommand<Result<IEnumerable<Product>>>;