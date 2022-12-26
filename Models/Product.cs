using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace challenge_20220626.Models{
    public class Product{

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public int code { get; set; }
        public string? barcode { get; set; }
        public string? status { get; set; }
        [BsonElement("imported_t")]
        public string? importedT { get; set; }
        public string? url { get; set; }
        [BsonElement("product_name")]
        public string? productName { get; set; }
        public string? quantity { get; set; }
        public string? categories { get; set; }
        public string? packaging { get; set; }
        public string? brands { get; set; }
        [BsonElement("image_url")]
        public string? imageUrl { get; set; }

    }
}