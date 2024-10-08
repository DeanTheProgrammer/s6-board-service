using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Enum;

namespace DTO
{
    public class StatusDTO
    { 
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public StatusEnum Status { get; set; }
    }

}
