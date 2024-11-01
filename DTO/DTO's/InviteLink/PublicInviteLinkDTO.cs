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
        public ProjectRoleEnum ReceivingRole { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string LinkCode { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectName { get; set; }
        public string InviterName { get; set; }
    }
}
