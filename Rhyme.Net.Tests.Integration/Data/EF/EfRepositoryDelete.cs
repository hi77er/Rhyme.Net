using Rhyme.Net.Core.Domain.MenuAggregate;
using Xunit;

namespace Rhyme.Net.Tests.Integration.Data.EF;

public class EfRepositoryDelete : BaseEfRepoTestFixture<Menu>
{
  [Fact]
  public async Task DeletesItemAfterAddingIt()
  {
    // add a Contributor
    var repository = GetRepository();
    var testMenuTitle = "Test Menu Title";
    var testStoreId = Guid.NewGuid();
    var testMenu = new Menu(testStoreId, testMenuTitle);
    await repository.AddAsync(testMenu);

    // delete the item
    await repository.DeleteAsync(testMenu);

    var checkMenu = await repository.GetByIdAsync(testMenu.Id);

    // verify it's no longer there
    Assert.Null(checkMenu);
  }
}
