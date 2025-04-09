using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace Rhyme.Net.Core.Domain.ProductAggregate;

public class Nutrient : EntityBase<Guid>
{
  public string Name { get; private set; }
  public string ShortName { get; private set; }
  public string Number { get; private set; }
  public string Status { get; private set; }
  public decimal Per100Grams { get; private set; }
  public decimal Per100Milliliters { get; private set; }
  public decimal PerServing { get; private set; }
  public decimal ServingSize { get; private set; }

  public Nutrient(string name, string shortName, string number, string status)
  {
    Name = Guard.Against.NullOrWhiteSpace(name, "name");
    ShortName = Guard.Against.NullOrWhiteSpace(shortName, "shortName");
    Number = Guard.Against.NullOrWhiteSpace(number, "number");
    Status = Guard.Against.NullOrWhiteSpace(status, "status");
  }

  public void SetPer100Grams(decimal per100grams)
  {
    Per100Grams = Guard.Against.Negative(per100grams, "per100grams");
  }

  public void SetPer100Milliliters(decimal per100milliliters)
  {
    Per100Milliliters = Guard.Against.Negative(per100milliliters, "per100milliliters");
  }

  public void SetPerServing(decimal perServing)
  {
    PerServing = Guard.Against.Negative(perServing, "perServing");
  }

  public void SetServingSize(decimal servingSize)
  {
    ServingSize = Guard.Against.Negative(servingSize, "servingSize");
  }

  public override string ToString()
  {
    var per100 = Per100Grams > Per100Milliliters ? Per100Grams : Per100Milliliters;
    return $"Nutrient: {Name}; Per 100: {per100}; Per serving: {PerServing};";
  }
}