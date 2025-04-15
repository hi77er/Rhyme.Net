using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Rhyme.Net.Core.EventSourcing;

namespace Rhyme.Net.Core.Converters;

public class EventStreamConverter : IPropertyConverter
{
  public DynamoDBEntry ToEntry(object value)
  {
    if (value is EventStream eventStream)
    {
      // Convert enum to its integer value (or string representation, depending on preference)
      return new Primitive(eventStream.ToString());
    }
    return string.Empty;
  }

  public object? FromEntry(DynamoDBEntry entry)
  {
    if (entry is Primitive primitive && primitive.Value != null)
    {
      // Convert the value back to the enum from the stored integer value
      return Enum.Parse(typeof(EventStream), primitive.AsString(), ignoreCase: true);
    }
    return null;
  }
}