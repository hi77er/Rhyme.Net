using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace Rhyme.Net.Core.Domain.MenuAggregate;

public class Brochure : EntityBase<Guid>
{
  public Guid MenuId { get; private set; }
  public string Title { get; private set; }
  public string Description { get; private set; } = string.Empty;

  private readonly List<MenuItem> _menuItems = new();
  public IEnumerable<MenuItem> MenuItems => _menuItems.AsReadOnly();

  public Brochure(Guid menuId, string title)
  {
    MenuId = Guard.Against.Expression(x => x == Guid.Empty, menuId, "menuId");
    Title = Guard.Against.NullOrWhiteSpace(title, "title");
  }

  public Brochure(Guid menuId, string title, string description)
  {
    MenuId = Guard.Against.Expression(x => x == Guid.Empty, menuId, "menuId");
    Title = Guard.Against.NullOrWhiteSpace(title, "title");
    Description = Guard.Against.NullOrWhiteSpace(description, "description");
  }

  public void AddMenuItem(MenuItem menuItem)
  {
    Guard.Against.Null(menuItem, "menuItem");
    _menuItems.Add(menuItem);
  }

  public override string ToString()
  {
    string menuItems = string.Join(", ", MenuItems.Select(i => i.ToString()));
    return $"Brochure Id: {Id}; Name: {Title}; Description: {Description}; Items: {menuItems};";
  }
}