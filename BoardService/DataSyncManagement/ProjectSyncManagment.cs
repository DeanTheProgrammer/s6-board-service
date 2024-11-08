using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.DTO_s.Project;
using Microsoft.Extensions.Logging;
using ProjectService.Interface;

namespace ProjectService.DataSyncManagement
{
    public class ProjectSyncManagment
    {
        private readonly ProjectSyncInterface _projectInterface;
        private readonly ILogger<ProjectSyncManagment> _logger;
        public ProjectSyncManagment(ProjectSyncInterface projectInterface, ILogger<ProjectSyncManagment> logger)
        {
            _projectInterface = projectInterface;
            _logger = logger;
        }

        public async Task UpdateProject(ProjectDTO NewProject)
        {
            ProjectDTO OldProject = await _projectInterface.GetProject(NewProject.Id);
            if (OldProject == null)
            {
                throw new Exception("Project not found");
            }

            if (NewProject != OldProject)
            {
                await _projectInterface.UpdateProject(NewProject);
            }
        }

        public async Task DeleteProject(ProjectDTO NewProject, string UserId)
        {
            ProjectDTO OldProject = await _projectInterface.GetProject(NewProject.Id);
            if(OldProject == null)
            {
                throw new Exception("Project not found");
            }

            if (OldProject.IsDeleted == false)
            {
                await _projectInterface.DeleteProject(NewProject.Id, DateTime.Now, UserId);
            }
        }

        public async Task CreateProject(ProjectDTO NewProject)
        {
            ProjectDTO OldProject = await _projectInterface.GetProject(NewProject.Id);
            if (OldProject != null)
            {
                throw new Exception("Project already exists");
            }
            else
            {
                await _projectInterface.CreateProject(NewProject);
            }
        }
    }
}
