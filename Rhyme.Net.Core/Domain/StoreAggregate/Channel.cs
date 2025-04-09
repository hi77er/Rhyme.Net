using Ardalis.SmartEnum;

namespace Rhyme.Net.Core.Domain.StoreAggregate;

public class Channel : SmartEnum<Channel>
{
  public static readonly Channel Delivery = new("Delivery", 0);
  public static readonly Channel EatIn = new("Eat In", 1);
  public static readonly Channel Collect = new("Collect", 2);

  protected Channel(string name, int value) : base(name, value) { }
}