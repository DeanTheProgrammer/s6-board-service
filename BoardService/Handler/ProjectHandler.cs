﻿using System.ComponentModel.DataAnnotations;
using ProjectService.Interface;
using CustomExceptions.ObjectExceptions;
using DTO;
using DTO.DTO_s.Project;
using DTO.DTO_s.InviteLink;
using DTO.Enum;
using Microsoft.Extensions.Logging;
using InfraRabbitMQ.Handler.DataSync;

namespace ProjectService.Handler
{
    public class ProjectHandler
    {
        private readonly ProjectDSInterface _ProjectDsInterface;
        private readonly ILogger<ProjectHandler> _logger;
        private readonly ProjectSyncHandler _syncHandler;

        public ProjectHandler(ProjectDSInterface ProjectDsInterface, ILogger<ProjectHandler>? logger, ProjectSyncHandler syncHandler)
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
            ProjectDTO result = await _ProjectDsInterface.GetProject(Id, UserId);

            return result;
        }

        public async Task<ProjectDTO> GetProject(string ProjectId, string UserId)
        {
            ProjectDTO project = await _ProjectDsInterface.GetProject(ProjectId, UserId);
            _syncHandler.PublishSyncObject(project);
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
