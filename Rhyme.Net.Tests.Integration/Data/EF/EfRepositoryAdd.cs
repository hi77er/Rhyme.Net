using Rhyme.Net.Core.Domain.MenuAggregate;
using Xunit;

namespace Rhyme.Net.Tests.Integration.Data.EF;

public class EfRepositoryAdd : BaseEfRepoTestFixture<Menu>
{
  [Fact]
  public async Task AddsMenuAndSetsId()
  {
    var testMenuStoreId = Guid.NewGuid();
    var testMenuTitle = "Test Menu Title";
    var repository = GetRepository();
    var testMenu = new Menu(testMenuStoreId, testMenuTitle);

    await repository.AddAsync(testMenu);

    var foundMenu = await repository.GetByIdAsync(testMenu.Id);

    Assert.True(foundMenu?.Id != default(Guid));
    Assert.Equal(testMenuStoreId, foundMenu?.StoreId);
    Assert.Equal(testMenuTitle, foundMenu?.Title);
  }
}