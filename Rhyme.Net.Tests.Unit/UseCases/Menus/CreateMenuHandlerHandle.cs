using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.MenuAggregate;
using Rhyme.Net.UseCases.Menus.Create;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Rhyme.Net.Tests.Unit.UseCases.Menus;

public class CreateMenuHandlerHandle
{
    private readonly Guid _testMenuStoreId = Guid.NewGuid();
    private readonly string _testMenuTitle = "Test Menu Title";
    private readonly IRepository<Menu> _repository = Substitute.For<IRepository<Menu>>();
    private CreateMenuHandler _handler;

    public CreateMenuHandlerHandle()
    {
        _handler = new CreateMenuHandler(_repository);
    }

    private Menu CreateTestMenu(Guid storeId, string title)
        => new Menu(storeId, title);

    [Fact]
    public async Task ReturnsSuccessGivenValidTitle()
    {
        var testMenu = CreateTestMenu(_testMenuStoreId, _testMenuTitle);

        _repository
            .AddAsync(Arg.Any<Menu>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(testMenu));

        var command = new CreateMenuCommand(_testMenuStoreId, _testMenuTitle);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }
}