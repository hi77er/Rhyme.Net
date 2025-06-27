using Amazon.DynamoDBv2.Model;
using Ardalis.SharedKernel;

public class DynamoDbEntity: HasDomainEventsBase
{
    /// <summary>
    /// Default constructor for DynamoDB
    /// </summary>
    public DynamoDbEntity() { }

    /// <summary>
    /// Converts the entity to a dictionary of attribute values for DynamoDB.
    /// </summary>
    /// <returns>A dictionary of attribute values.</returns>
    public virtual Dictionary<string, AttributeValue> ToAttributeValues()
    {
        return new Dictionary<string, AttributeValue>();
    }
}