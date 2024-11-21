using System.ComponentModel.DataAnnotations;
using DTO.Enum;

namespace DTO.DTO_s.Project
{
    public class ProjectDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public SprintTimeEnum SprintTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<UserDTO> Users { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string DeletedBy { get; set; }
    }
}
