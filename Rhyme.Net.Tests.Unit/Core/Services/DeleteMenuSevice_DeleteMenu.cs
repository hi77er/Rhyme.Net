using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.MenuAggregate;
using Rhyme.Net.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Rhyme.Net.Tests.Unit.Core.Services;

public class DeleteMenuSevice_DeleteMenu
{
  private readonly IRepository<Menu> _repository = Substitute.For<IRepository<Menu>>();
  private readonly IMediator _mediator = Substitute.For<IMediator>();
  private readonly ILogger<DeleteMenuService> _logger = Substitute.For<ILogger<DeleteMenuService>>();

  private readonly DeleteMenuService _service;

  public DeleteMenuSevice_DeleteMenu()
  {
    _service = new DeleteMenuService(_repository, _mediator, _logger);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenCantFindMenu()
  {
    var nonExistingMenuId = Guid.NewGuid();
    var result = await _service.DeleteMenu(nonExistingMenuId);

    Assert.Equal(Ardalis.Result.ResultStatus.NotFound, result.Status);
  }
}
