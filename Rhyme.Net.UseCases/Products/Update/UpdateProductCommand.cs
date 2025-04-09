using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Rhyme.Net.UseCases.Products.Update;

public record UpdateProductCommand(int ProductId, string NewName) : ICommand<Result<ProductDTO>>;
