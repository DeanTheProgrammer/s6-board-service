using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Enum;

namespace DTO.DTO_s.InviteLink
{
    public class CreateInviteLinkDTO
    {
        public string BoardId { get; set; }
        public BoardRoleEnum BoardRole { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
