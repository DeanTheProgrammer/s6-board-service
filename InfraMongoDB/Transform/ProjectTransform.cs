using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DTO.DTO_s.InviteLink;
using DTO.DTO_s.Project;
using DTO.Enum;
using Models.Models;
using MongoDB.Bson;
using SprintTimeEnum = Models.Enum.SprintTimeEnum;

namespace InfraMongoDB.Transform
{
    public class ProjectTransform
    {
        public static ProjectDTO ToDTO(ProjectModel Project)
        {
            ProjectDTO result = new ProjectDTO()
            {
                Id = Project.Id.ToString(),
                Name = Project.Name,
                Description = Project.Description,
                SprintTime = (DTO.Enum.SprintTimeEnum)Project.SprintTime,
                CreatedAt = Project.CreatedAt,
                CreatedBy = Project.CreatedBy,
                Users = new List<UserDTO>(),
                IsDeleted = Project.IsDeleted,
                DeletedAt = Project.DeletedAt
            };


            foreach (var user in Project.Users)
            {
                result.Users.Add(new UserDTO()
                {
                    Id = user.Id.ToString(),
                    Role = (ProjectRoleEnum)user.Role,
                    Nickname = user.Nickname,
                    TeamRole = user.TeamRole
                });
            }

            return result;
        }

        public static UserModel ToModel(UserDTO User)
        {
            UserModel result = new UserModel()
            {
                Id = User.Id,
                Role = (Models.Enum.ProjectRoleEnum)User.Role,
                Nickname = User.Nickname,
                TeamRole = User.TeamRole
            };
            return result;
        }

        public static ProjectModel ToModel(ProjectDTO Project)
        {
            ProjectModel result = new ProjectModel()
            {
                Id = ObjectId.Parse(Project.Id),
                Name = Project.Name,
                Description = Project.Description,
                SprintTime = (SprintTimeEnum) Project.SprintTime,
                CreatedAt = Project.CreatedAt,
                CreatedBy = Project.CreatedBy,
                Users = new List<UserModel>(),
                IsDeleted = Project.IsDeleted,
                DeletedAt = Project.DeletedAt
            };



            foreach (var user in Project.Users)
            {
                result.Users.Add(new UserModel()
                {
                    Id = user.Id,
                    Role = (Models.Enum.ProjectRoleEnum)user.Role,
                    Nickname = user.Nickname,
                    TeamRole = user.TeamRole
                });
            }

            return result;
        }


        public static ProjectModel ToModelUpdate(ProjectModel Old, UpdateProjectDTO newProject)
        {

            Old.Name = newProject.Name;
            Old.Description = newProject.Description;
            Old.SprintTime = (SprintTimeEnum)newProject.SprintTime;

            return Old;
        }

        public static SmallProjectDTO ToSmallProjectDTO(ProjectModel model)
        {
            SmallProjectDTO result = new SmallProjectDTO()
            {
                Id = model.Id.ToString(),
                Name = model.Name,
                Description = model.Description,
            };
            return result;
        }

    }
}
