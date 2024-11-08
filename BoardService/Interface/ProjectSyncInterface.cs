using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.DTO_s.Project;

namespace ProjectService.Interface
{
    public interface ProjectSyncInterface
    {
        public Task<ProjectDTO> GetProject(string ProjectId);
        public Task UpdateProject(ProjectDTO Project);
        public Task DeleteProject(string ProjectId, DateTime DeleteTime, string UserId);
        public Task CreateProject (ProjectDTO Project);
    }
}
