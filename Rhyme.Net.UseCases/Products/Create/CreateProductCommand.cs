using Ardalis.Result;

namespace Rhyme.Net.UseCases.Products.Create;

/// <summary>
/// Create a new Product.
/// </summary>
/// <param name="Name"></param>
public record CreateProductCommand(string Name) : Ardalis.SharedKernel.ICommand<Result<Guid>>;