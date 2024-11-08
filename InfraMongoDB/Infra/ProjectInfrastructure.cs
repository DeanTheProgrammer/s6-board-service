using ProjectService.Interface;
using Models.Enum;
using Models.Models;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using CustomExceptions.ObjectExceptions;
using DTO;
using DTO.DTO_s.InviteLink;
using DTO.DTO_s.Project;
using InfraMongoDB.Transform;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace InfraMongoDB.Infra
{
    public class ProjectInfrastructure : ProjectDSInterface, ProjectSyncInterface
    {
        private readonly IMongoCollection<ProjectModel> _ProjectCollection;

        public ProjectInfrastructure(IOptions<MongoDBSettings> options)
        {

            MongoDBSettings Settings = options.Value;
            MongoClient client = new MongoClient(Settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(Settings.DatabaseName);
            _ProjectCollection = database.GetCollection<ProjectModel>("Project");
        }

        public async Task AddUserToProject(string ProjectId, UserDTO User)
        {
            if (string.IsNullOrEmpty(ProjectId))
            {
                throw new ValidationException("ProjectId cannot be null");
            }

            ProjectModel Project =  await _ProjectCollection.Find(b => b.Id == ObjectId.Parse(ProjectId)).FirstOrDefaultAsync();
            if (Project == null)
            {
                throw new NotFoundException("Project not found");
            }

            Project.Users.Add(Transform.ProjectTransform.ToModel(User));

            await _ProjectCollection.ReplaceOneAsync(b => b.Id == ObjectId.Parse(ProjectId), Project);
        }

        public async Task<string> CreateProject(string UserId, CreateProjectDTO input)
        {
            if (string.IsNullOrEmpty(input.Name))
            {
                throw new ValidationException("Name cannot be null");
            }

            ProjectModel NewProject = new ProjectModel()
            {
                Name = input.Name,
            };

            if (!string.IsNullOrEmpty(input.Description))
            {
                NewProject.Description = input.Description;
            }
            else
            {
                NewProject.Description = "";
            }

            NewProject.CreatedBy = UserId;
            NewProject.CreatedAt = DateTime.Now;
            NewProject.SprintTime = SprintTimeEnum.None;
            NewProject.Users = new List<UserModel>();
            NewProject.Users.Add(new UserModel() { Id = UserId, Role = ProjectRoleEnum.Admin });

            NewProject.IsDeleted = false;

            await _ProjectCollection.InsertOneAsync(NewProject);
            return NewProject.Id.ToString();
        }

        public async Task CreateProject(ProjectDTO Project)
        {
            ProjectModel NewProject = Transform.ProjectTransform.ToModel(Project);

            await _ProjectCollection.InsertOneAsync(NewProject);
        }

        public async Task DeleteProject(string UserId, string ProjectId)
        {
            ProjectModel Project = await _ProjectCollection.Find(b => b.Id == ObjectId.Parse(ProjectId)).FirstOrDefaultAsync();
            if (Project == null)
            {
                throw new NotFoundException("Project is not found");
            }

            Project.IsDeleted = true;
            Project.DeletedAt = DateTime.Now;
            Project.DeletedBy = UserId;

            await _ProjectCollection.ReplaceOneAsync(b => b.Id == ObjectId.Parse(ProjectId), Project);
        }

        public async Task DeleteProject(string ProjectId, DateTime DeleteTime, string UserId)
        {
            ProjectModel Project = await _ProjectCollection.Find(b => b.Id == ObjectId.Parse(ProjectId)).FirstOrDefaultAsync();
            if (Project == null)
            {
                throw new NotFoundException("Project is not found");
            }
            Project.IsDeleted = true;
            Project.DeletedAt = DeleteTime;
            Project.DeletedBy = UserId;
            await _ProjectCollection.ReplaceOneAsync(b => b.Id == ObjectId.Parse(ProjectId), Project);
        }

        public async Task<List<ProjectDTO>> GetActiveProjects(string UserId)
        {
            List<ProjectModel> Projects = await _ProjectCollection.Find(b => b.Users.Any(u => u.Id == UserId) && b.IsDeleted == false).ToListAsync();

            List<ProjectDTO> result = new List<ProjectDTO>();
            foreach (var Project in Projects)
            {
                result.Add(ProjectTransform.ToDTO(Project));
            }
            return result;
        }

        public async Task<ProjectDTO> GetProject(string ProjectId)
        {
            ProjectModel result = await _ProjectCollection.Find(b => b.Id == ObjectId.Parse(ProjectId)).FirstOrDefaultAsync();
            if (result == null)
            {
                return null;
            }
            else
            {
                return Transform.ProjectTransform.ToDTO(result);
            }
        }

        public async Task<List<ProjectDTO>> GetProjectArchived(string UserId)
        {
            List<ProjectModel> Projects = await _ProjectCollection.Find(b =>
                b.Users.Any(u => u.Id == UserId) && b.DeletedAt.Value.AddDays(30) > DateTime.Now).ToListAsync();

            List<ProjectDTO> result = new List<ProjectDTO>();
            foreach (ProjectModel Project in Projects)
            {
                result.Add(Transform.ProjectTransform.ToDTO(Project));
            }
            return result;
        }

        public async Task<DateTime> GetProjectDeletedDate(string ProjectId)
        {
            ProjectModel Project = await _ProjectCollection.Find(b => b.Id == ObjectId.Parse(ProjectId) && b.IsDeleted == true)
                .FirstOrDefaultAsync();

            if (Project.DeletedAt.Value == null)
            {
                throw new Exception("This should not be possible");
            }
            return Project.DeletedAt.Value;
        }

        public async Task<List<ProjectDTO>> GetProjects(string UserId)
        { 
            List<ProjectDTO> Result = new List<ProjectDTO>();
            List<ProjectModel> models = await _ProjectCollection.Find(b => b.Users.Any(u => u.Id == UserId)).ToListAsync();
            foreach (var model in models)
            {
                Result.Add(Transform.ProjectTransform.ToDTO(model));
            }
            return Result;
        }

        public async Task<SmallProjectDTO> GetSmallProject(string ProjectId)
        {
            
            ProjectModel model = await _ProjectCollection.Find(b => b.Id == ObjectId.Parse(ProjectId)).FirstOrDefaultAsync();

            return Transform.ProjectTransform.ToSmallProjectDTO(model);
        }

        public async Task<List<SmallProjectDTO>> GetSmallProjects(string UserId)
        {
            List<SmallProjectDTO> Result = new List<SmallProjectDTO>();
            List<ProjectModel> models = await _ProjectCollection.Find(b => b.Users.Any(u => u.Id == UserId)).ToListAsync();
            foreach (ProjectModel model in models)
            {
                Result.Add(Transform.ProjectTransform.ToSmallProjectDTO(model));
            }

            return Result;
        }

        public async Task RemoveUserFromProject(string ProjectId, string UserId)
        {
            ProjectModel Project = await _ProjectCollection.Find(b => b.Id == ObjectId.Parse(ProjectId)).FirstOrDefaultAsync();
            UserModel user = Project.Users.Find(u => u.Id == UserId);

            if (user == null)
            {
                throw new NotFoundException("User doesn't exist in Project");
            }

            Project.Users.Remove(user);

            await _ProjectCollection.ReplaceOneAsync(b => b.Id == ObjectId.Parse(ProjectId), Project);

        }

        public async Task UnDeleteProject(string ProjectId)
        {
            ProjectModel Project = await _ProjectCollection.Find(b => b.Id == ObjectId.Parse(ProjectId)).FirstOrDefaultAsync();

            Project.DeletedAt = DateTime.MinValue;
            Project.DeletedBy = null;
            Project.IsDeleted = false;

            await _ProjectCollection.ReplaceOneAsync(b => b.Id == ObjectId.Parse(ProjectId), Project);
        }

        public async Task UpdateProject(string ProjectId, UpdateProjectDTO Project)
        {
            ProjectModel OldProject = await _ProjectCollection.Find(b => b.Id == ObjectId.Parse(ProjectId)).FirstOrDefaultAsync();
            ProjectModel NewProject = Transform.ProjectTransform.ToModelUpdate(OldProject, Project);

            await _ProjectCollection.ReplaceOneAsync(b => b.Id == ObjectId.Parse(ProjectId), NewProject);

        }

        public async Task UpdateProject(ProjectDTO Project)
        {
            ProjectModel UpdateModel = Transform.ProjectTransform.ToModel(Project);
            await _ProjectCollection.ReplaceOneAsync(b => b.Id == ObjectId.Parse(Project.Id), UpdateModel);
        }

        public async Task UpdateUserRole(string ProjectId, string UserId, DTO.Enum.ProjectRoleEnum Role)
        {
            ProjectModel Project = await _ProjectCollection.Find(b => b.Id == ObjectId.Parse(ProjectId)).FirstOrDefaultAsync();
            if (Project == null)
            {
                throw new NotFoundException("Project not found");
            }

            foreach (UserModel user in Project.Users)
            {
                if (user.Id == UserId)
                {
                    user.Role = (ProjectRoleEnum)Role;
                }
            }

            await _ProjectCollection.ReplaceOneAsync(b => b.Id == ObjectId.Parse(ProjectId), Project);

        }
    }
}
