using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DriverService.Models
{
    public class DemoDriverReservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("driverId")]
        public Guid DriverId { get; set; }

        [BsonElement("sagaId")]
        public string SagaId { get; set; } = string.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = "Reserved";

        [BsonElement("reservedAt")]
        public DateTime ReservedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("releasedAt")]
        public DateTime? ReleasedAt { get; set; }
    }
}
