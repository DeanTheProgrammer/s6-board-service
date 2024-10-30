using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Enum;

namespace DTO.DTO_s.InviteLink
{
    public class PublicInviteLinkDTO
    {
        public string Code { get; set; }
        public BoardRoleEnum ReceivingRole { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string LinkCode { get; set; }
        public string BoardDescription { get; set; }
        public string BoardName { get; set; }
        public string InviterName { get; set; }
    }
}
