using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Models
{
    public class ColumnsModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Description")]
        public string Description { get; set; }
        [BsonElement("OrderNumber")]
        public int OrderNumber { get; set; }
        [BsonElement("StatusId")]
        public string StatusId { get; set; }
    }
}
