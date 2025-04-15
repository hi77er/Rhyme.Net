using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace Rhyme.Net.Core.Converters;

public class GuidConverter : IPropertyConverter
{
  public DynamoDBEntry ToEntry(object value)
    => value != null ? new Primitive(value.ToString()) : string.Empty;

  public object FromEntry(DynamoDBEntry entry)
    => entry?.AsString() != null ? Guid.Parse(entry.AsString()) : Guid.Empty;

}