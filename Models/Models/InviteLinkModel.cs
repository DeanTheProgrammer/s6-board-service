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
    public class InviteLinkModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("ProjectId")]
        public string ProjectId { get; set; }
        [BsonElement("LinkCode")]
        public string LinkCode { get; set; }
        [BsonElement("CreatedBy")]
        public string CreatedBy { get; set; }
        [BsonElement("ReceivingRole")]
        public ProjectRoleEnum ReceivingRole { get; set; }
        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        [BsonElement("ExpiresAt")]
        public DateTime ExpiresAt { get; set; }
        [BsonElement("IsDeleted")]
        public bool IsDeleted { get; set; }
    }
}
