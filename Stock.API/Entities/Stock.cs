using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Stock.API.Entities
{
    public class Stock
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.CSharpLegacy)]
        [BsonElement(Order = 0)]
        public Guid Id { get; set; }

        [BsonGuidRepresentation(GuidRepresentation.CSharpLegacy)]
        [BsonElement(Order = 1)]
        public Guid ProductId { get; set; }

        [BsonElement(Order = 2)]
        [BsonRepresentation(BsonType.Int32)]
        public int Count { get; set; }
    }
}
