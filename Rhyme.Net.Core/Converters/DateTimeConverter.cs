using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.DataModel;
using System;

public class DateTimeConverter : IPropertyConverter
{
    public DynamoDBEntry ToEntry(object value)
    {
        if (value is DateTime dateTime)
        {
            // Convert DateTime to ISO 8601 string format for DynamoDB storage
            // "o" stands for round-trip date/time format (ISO 8601)
            return new Primitive(dateTime.ToString("o")); 
        }
        return string.Empty;
    }

    public object FromEntry(DynamoDBEntry entry)
    {
        if (entry is Primitive primitive && !string.IsNullOrEmpty(primitive.AsString()))
        {
            // Convert ISO 8601 string back to DateTime
            return DateTime.Parse(primitive.AsString(), null, System.Globalization.DateTimeStyles.RoundtripKind);
        }
        return DateTime.MinValue;
    }
}