using Rhyme.Net.Core.Domain.MenuAggregate;
using Xunit;

namespace Rhyme.Net.Tests.Unit.Core.Domain.MenuAggregate;

public class MenuConstructor
{
  private readonly Guid _testMenuStoreId = Guid.NewGuid();
  private readonly string _testMenuTitle = "test name";
  private Menu? _testMenu;

  [Fact]
  public void InitializesMenu()
  {
    _testMenu = new Menu(_testMenuStoreId, _testMenuTitle);

    Assert.Equal(_testMenuStoreId, _testMenu.StoreId);
    Assert.Equal(_testMenuTitle, _testMenu.Title);
  }
}