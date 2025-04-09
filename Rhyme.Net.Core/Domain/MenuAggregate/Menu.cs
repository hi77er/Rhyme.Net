using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.MenuAggregate.Events;

namespace Rhyme.Net.Core.Domain.MenuAggregate;

public class Menu : EntityBase<Guid>, IAggregateRoot
{
  public Guid StoreId { get; private set; }
  public string Title { get; private set; }
  public string? Description { get; private set; }
  private readonly List<Brochure> _brochures = new();
  public IEnumerable<Brochure> Brochures => _brochures.AsReadOnly();

  public Menu(Guid storeId, string title)
  {
    StoreId = Guard.Against.Expression(x => x == Guid.Empty, storeId, "storeId");
    Title = Guard.Against.NullOrWhiteSpace(title, "title");
  }

  public Menu(Guid storeId, string title, string description)
  {
    StoreId = Guard.Against.Expression(x => x == Guid.Empty, storeId, "storeId");
    Title = Guard.Against.NullOrWhiteSpace(title, "title");
    Description = Guard.Against.NullOrWhiteSpace(description, "description");
  }

  public void UpdateTitle(string newTitle)
  {
    Guard.Against.NullOrWhiteSpace(newTitle, "newTitle");
    var oldTitle = Title;
    Title = newTitle;

    var domainEvent = new MenuTitleUpdatedEvent(this, oldTitle);
    RegisterDomainEvent(domainEvent);
  }

  public void AddBrochure(Brochure brochure)
  {
    Guard.Against.Null(brochure, "brochure");
    _brochures.Add(brochure);
  }

  public override string ToString()
  {
    var brochures = string.Join(", ", Brochures.Select(i => i.ToString()));
    return $"Menu: {Title}; Description: {Description}; Brochures: {brochures};";
  }
}
