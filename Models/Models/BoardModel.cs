using Models.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Models
{
    public class BoardModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Description")]
        public string Description { get; set; }
        [BsonElement("SprintTime")]
        public SprintTimeEnum SprintTime { get; set; }
        [BsonElement("CreatedBy")]
        public string CreatedBy { get; set; }
        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        [BsonElement("Users")]
        public List<UserModel> Users { get; set; }
        [BsonElement("IsDeleted")]
        public bool IsDeleted { get; set; }
        [BsonElement("DeletedAt")]
        public DateTime? DeletedAt { get; set; }
        [BsonElement("DeletedBy")]
        public string DeletedBy { get; set; }

    }
}
