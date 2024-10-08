using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Models
{
    public class StatusModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Description")]
        public string Description { get; set; }
        [BsonElement("Status")]
        public StatusEnum Status { get; set; }

    }
}
