using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Rhyme.Net.Core.Sourcing;

namespace Rhyme.Net.Core.Converters;

public class EventIssuerConverter : IPropertyConverter
{
  public DynamoDBEntry ToEntry(object value)
  {
    if (value is EventIssuer eventIssuer)
      return new Primitive(eventIssuer.ToString());

    return string.Empty;
  }

  public object? FromEntry(DynamoDBEntry entry)
  {
    if (entry is Primitive primitive && primitive.Value != null)
    {
      return Enum.Parse(typeof(EventIssuer), primitive.AsString(), ignoreCase: true);
    }
    return null;
  }
}