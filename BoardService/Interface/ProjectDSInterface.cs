using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DTO.DTO_s.Project;
using DTO.Enum;


namespace ProjectService.Interface
{
    public interface ProjectDSInterface
    {
        public Task AddUserToProject(string ProjectId, UserDTO User);
        public Task RemoveUserFromProject(string ProjectId, string UserId);
        public Task UpdateUserRole(string ProjectId, string UserId, ProjectRoleEnum Role);
        public Task<string> CreateProject(string UserId, CreateProjectDTO Project);
        public Task UpdateProject(string ProjectId, UpdateProjectDTO Project);
        public Task DeleteProject(string UserId, string ProjectId);
        public Task UnDeleteProject(string ProjectId);
        public Task<List<ProjectDTO>> GetActiveProjects(string UserId);
        public Task<List<ProjectDTO>> GetProjectArchived(string UserId);
        public Task<DateTime> GetProjectDeletedDate(string ProjectId);
        public Task<ProjectDTO> GetProject(string ProjectId, string UserId);
        public Task<List<ProjectDTO>> GetProjects(string UserId);
        public Task<SmallProjectDTO> GetSmallProject(string ProjectId);
        public Task<List<SmallProjectDTO>> GetSmallProjects(string UserId);
    }
}
