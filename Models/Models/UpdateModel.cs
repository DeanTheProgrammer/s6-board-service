using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Models
{
    public class UpdateModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Description")]
        public string Description { get; set; }
        [BsonElement("TimeStamp")]
        public DateTime TimeStamp { get; set; }
        [BsonElement("ByUserId")]
        public string ByUserId { get; set; }
    }
}
