using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO_s
{
    public class AuthorizationUserDTO
    {
        public string UserId { get; set; }
        public List<string> Roles { get; set; }
        public string Auth0Id { get; set; }
    }
}
