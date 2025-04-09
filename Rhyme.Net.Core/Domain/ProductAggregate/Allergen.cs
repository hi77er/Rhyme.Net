using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace Rhyme.Net.Core.Domain.ProductAggregate;

public class Allergen : EntityBase<Guid>
{
  public string Name { get; private set; }
  public string ShortName { get; private set; }
  public string Number { get; private set; }
  public string Status { get; private set; }
  public Guid SourcingFacilityId { get; private set; }
  public AllergensContainType AllergensContainType { get; private set; }
  public SensitivitiesContainType SensitivitiesContainType { get; private set; }
  public string ShelfLife { get; private set; }
  public string ShelfLifeOnceOpened { get; private set; }
  public IEnumerable<ComplyWith>? CompliesWith { get; private set; }

  public Allergen(string name, string shortName, string number, string status, Guid sourcingFacilityId, AllergensContainType allergensContainType, SensitivitiesContainType sensitivitiesContainType, string shelfLife, string shelfLifeOnceOpened)
  {
    Name = Guard.Against.NullOrWhiteSpace(name, "name");
    ShortName = Guard.Against.NullOrWhiteSpace(shortName, "shortName");
    Number = Guard.Against.NullOrWhiteSpace(number, "number");
    Status = Guard.Against.NullOrWhiteSpace(status, "status");
    SourcingFacilityId = Guard.Against.Expression(x => x == Guid.Empty, sourcingFacilityId, "sourcingFacilityId");
    AllergensContainType = allergensContainType;
    SensitivitiesContainType = sensitivitiesContainType;
    ShelfLife = Guard.Against.NullOrWhiteSpace(shelfLife, "shelfLife");
    ShelfLifeOnceOpened = Guard.Against.NullOrWhiteSpace(shelfLifeOnceOpened, "shelfLifeOnceOpened");
  }

  public void SetCompliesWith(IEnumerable<ComplyWith> compliesWiths)
  {
    CompliesWith = Guard.Against.Null(compliesWiths, "compliesWith");
  }

  public override string ToString()
  {
    return $"{Name} - Allergens: {AllergensContainType.Name}; Sensitivities: {SensitivitiesContainType.Name};";
  }
}
