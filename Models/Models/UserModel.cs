using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enum;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Models
{
    public class UserModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("Nickname")]
        public string Nickname { get; set; }
        [BsonElement("TeamRole")]
        public string TeamRole { get; set; }
        [BsonElement("Role")]
        public BoardRoleEnum Role { get; set; }
    }
}
