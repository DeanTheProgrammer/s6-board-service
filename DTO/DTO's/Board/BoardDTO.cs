﻿using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class BoardDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<UpdateDTO> Updates { get; set; }
        public List<UserDTO> Users { get; set; }
        public List<ColumnsDTO> Columns { get; set; }
        public List<StatusDTO> Status { get; set; }
    }
}