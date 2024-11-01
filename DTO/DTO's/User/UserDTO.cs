using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Enum;

namespace DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Nickname { get; set; }
        public string TeamRole { get; set; }
        public ProjectRoleEnum Role { get; set; }
    }
}
