using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Rhyme.Net.UseCases.Products.Delete;

public record DeleteProductCommand(Guid ProductId) : ICommand<Result>;
