using Ardalis.SmartEnum;

namespace Rhyme.Net.Core.Domain.OrderAggregate;

public class OrderStatus : SmartEnum<OrderStatus>
{
  public static readonly OrderStatus Initiated = new("Initiated", 0);
  public static readonly OrderStatus Submitted = new("Submitted", 1);
  public static readonly OrderStatus InProgress = new("In progress", 2);
  public static readonly OrderStatus Complete = new("Complete", 3);

  protected OrderStatus(string name, int value) : base(name, value) { }
}