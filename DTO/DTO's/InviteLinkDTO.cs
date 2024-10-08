using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Enum;

namespace DTO
{
    public class InviteLinkDTO
    {
        public string Id { get; set; }
        public string BoardId { get; set; }
        public string LinkCode { get; set; }
        public string CreatedBy { get; set; }
        public BoardRoleEnum ReceivingRole { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }

    }
}
