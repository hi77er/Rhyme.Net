using Rhyme.Net.Core.Domain.MenuAggregate;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Runtime.CompilerServices;

namespace Rhyme.Net.Tests.Integration.Data.EF;

public class EfRepositoryUpdate : BaseEfRepoTestFixture<Menu>
{
  [Fact]
  public async Task UpdatesItemAfterAddingIt()
  {
    // add a Contributor
    var repository = GetRepository();
    var testMenuTitle = "Test Menu Title";
    var testStoreId = Guid.NewGuid();
    var testMenu = new Menu(testStoreId, testMenuTitle);

    await repository.AddAsync(testMenu);

    // detach the item so we get a different instance
    _dbContext.Entry(testMenu).State = EntityState.Detached;

    // fetch the item and update its title
    var addedMenu = await repository.GetByIdAsync(testMenu.Id);

    Assert.NotNull(addedMenu);
    Assert.NotSame(testMenu, addedMenu);

    var newTestTitle = "New Test Menu Title";
    addedMenu.UpdateTitle(newTestTitle);

    // Update the item
    await repository.UpdateAsync(addedMenu);

    // Fetch the updated item
    var updatedItem = await repository.GetByIdAsync(addedMenu.Id);

    Assert.NotNull(updatedItem);
    Assert.NotEqual(testMenu.Title, updatedItem?.Title);
    Assert.Equal(testMenu.Id, updatedItem?.Id);
  }
}
