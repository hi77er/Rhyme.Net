using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.ProductAggregate.Events;

namespace Rhyme.Net.Core.Domain.ProductAggregate;

public class Product : EntityBase<Guid>, IAggregateRoot
{
  public string Name { get; private set; }
  public string? Description { get; private set; }
  public IEnumerable<Nutrient> Nutritions { get; private set; } = new List<Nutrient>();
  public IEnumerable<Allergen> Allergens { get; private set; } = new List<Allergen>();

  public Product(string name)
  {
    Name = Guard.Against.NullOrWhiteSpace(name, "name");
  }

  public Product(string name, string description, IEnumerable<Nutrient> nutritions, IEnumerable<Allergen> allergens)
  {
    Name = Guard.Against.NullOrWhiteSpace(name, "name");
    Description = Guard.Against.NullOrWhiteSpace(description, "description");
    Nutritions = Guard.Against.Null(nutritions, "nutritions");
    Allergens = Guard.Against.Null(allergens, "allergens");
  }

  public void UpdateName(string newName)
  {
    Guard.Against.NullOrWhiteSpace(newName, "newName");
    var oldName = Name;
    Name = newName;

    var domainEvent = new ProductNameUpdatedEvent(this, oldName);
    RegisterDomainEvent(domainEvent);
  }

  public override string ToString()
  {
    return $"Product Id: {Id}; Name: {Name}; Description: {Description}";
  }
}
