using System.Text.Json;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace Rhyme.Net.Core.Converters;

public class ListConverter<T> : IPropertyConverter
{
  public DynamoDBEntry? ToEntry(object value)
  {
    if (value is IList<T> list)
    {
      var dynamoList = new DynamoDBList();
      foreach (var item in list)
      {
        dynamoList.Add(new Primitive(JsonSerializer.Serialize(item)));
      }
      return dynamoList;
    }
    return null;
  }

  public object? FromEntry(DynamoDBEntry entry)
  {
    if (entry is DynamoDBList dynamoList)
    {
      var list = new List<T>();
      foreach (var item in dynamoList.Entries)
      {
        if (item is Primitive primitive && primitive.Value != null)
        {
          list.Add((T)JsonSerializer.Deserialize(primitive.Value.ToString()!, typeof(T))!);
        }
      }
      return list;
    }
    return null;
  }
}