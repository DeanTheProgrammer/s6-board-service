using System.ComponentModel.DataAnnotations;
using ProjectService.Interface;
using CustomExceptions.ObjectExceptions;
using DTO;
using DTO.DTO_s.Project;
using DTO.DTO_s.InviteLink;
using DTO.Enum;
using Microsoft.Extensions.Logging;
using InfraRabbitMQ.Handler.DataSync;
using InfraRabbitMQ.Object;

namespace ProjectService.Handler
{
    public class ProjectHandler
    {
        private readonly ProjectDSInterface _ProjectDsInterface;
        private readonly ILogger<ProjectHandler> _logger;
        private readonly ProjectSyncPublisher _syncHandler;

        public ProjectHandler(ProjectDSInterface ProjectDsInterface, ILogger<ProjectHandler>? logger, ProjectSyncPublisher syncHandler)
        {
            this._ProjectDsInterface = ProjectDsInterface;
            _logger = logger;
            _syncHandler = syncHandler;
        }

        public async Task<ProjectDTO> CreateProject(string UserId, CreateProjectDTO Project)
        {
            if (String.IsNullOrEmpty(Project.Name))
            {
                throw new ValidationException("Name cannot be null");
            }

            string Id = await _ProjectDsInterface.CreateProject(UserId, Project);
            ProjectDTO result = await _ProjectDsInterface.GetProject(Id);

            return result;
        }

        public async Task<ProjectDTO> GetProject(string ProjectId, string UserId)
        {
            ProjectDTO project = await _ProjectDsInterface.GetProject(ProjectId);
            if (!project.Users.Any(U => U.Id == UserId))
            {
                throw new UnauthorizedAccessException("You don't have access to this project");
            }
            return project;
        }

        public async Task<List<ProjectDTO>> GetProjects(string UserId)
        {
            return await _ProjectDsInterface.GetProjects(UserId);
        }

        public async Task<SmallProjectDTO> GetSmallProject(string ProjectId)
        {
            return await _ProjectDsInterface.GetSmallProject(ProjectId);
        }

        public async Task<List<SmallProjectDTO>> GetSmallProjects(string UserId)
        {
            return await _ProjectDsInterface.GetSmallProjects(UserId);
        }

    }
}
