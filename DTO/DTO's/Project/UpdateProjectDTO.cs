﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Enum;

namespace DTO.DTO_s.Project
{
    public class UpdateProjectDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public SprintTimeEnum SprintTime { get; set; }
    }
}
