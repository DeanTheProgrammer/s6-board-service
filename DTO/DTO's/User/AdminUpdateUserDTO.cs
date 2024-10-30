using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Enum;

namespace DTO.DTO_s.User
{
    public class AdminUpdateUserDTO
    {
        public string Nickname { get; set; }
        public string TeamRole { get; set; }
        public BoardRoleEnum Role { get; set; }
    }
}
