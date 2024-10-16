using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DTO.DTO_s.Board;
using DTO.Enum;
using Models.Models;
using MongoDB.Bson;

namespace InfraMongoDB.Transform
{
    public class BoardTransform
    {
        public static BoardDTO ToDTO(BoardModel board)
        {
            BoardDTO result = new BoardDTO()
            {
                Id = board.Id.ToString(),
                Name = board.Name,
                Description = board.Description,
                CreatedAt = board.CreatedAt,
                CreatedBy = board.CreatedBy,
                Columns = new List<ColumnsDTO>(),
                Updates = new List<UpdateDTO>(),
                Status = new List<StatusDTO>(),
                Users = new List<UserDTO>(),
                IsDeleted = board.IsDeleted,
                DeletedAt = board.DeletedAt
            };


            foreach(var column in board.Columns)
            {
                result.Columns.Add(new ColumnsDTO
                {
                    Id = column.Id.ToString(),
                    Name = column.Name,
                    Description = column.Description,
                    OrderNumber = column.OrderNumber,
                    StatusId = column.StatusId.ToString()
                });
            }

            foreach (var statusModel in board.Status)
            {
                result.Status.Add(new StatusDTO()
                {
                    Id = statusModel.Id.ToString(),
                    Name = statusModel.Name,
                    Description = statusModel.Description,
                    Status = (StatusEnum)statusModel.Status
                });
            }

            foreach (var update in board.Updates)
            {
                result.Updates.Add(new UpdateDTO()
                {
                    Id = update.Id.ToString(),
                    Description = update.Description,
                    Name = update.Name,
                    TimeStamp = update.TimeStamp,
                    ByUserId = update.ByUserId
                });
            }

            foreach (var user in board.Users)
            {
                result.Users.Add(new UserDTO()
                {
                    Id = user.Id.ToString(),
                    Role = (BoardRoleEnum)user.Role,
                    Nickname = user.Nickname,
                    TeamRole = user.TeamRole
                });
            }

            return result;
        }

        public static BoardModel ToModel(BoardDTO board)
        {
            BoardModel result = new BoardModel()
            {
                Id = ObjectId.Parse(board.Id),
                Name = board.Name,
                Description = board.Description,
                CreatedAt = board.CreatedAt,
                CreatedBy = board.CreatedBy,
                Columns = new List<ColumnsModel>(),
                Updates = new List<UpdateModel>(),
                Status = new List<StatusModel>(),
                Users = new List<UserModel>(),
                IsDeleted = board.IsDeleted,
                DeletedAt = board.DeletedAt
            };

            foreach (var column in board.Columns)
            {
                result.Columns.Add(new ColumnsModel
                {
                    Id = ObjectId.Parse(column.Id),    
                    Name = column.Name,
                    Description = column.Description,
                    OrderNumber = column.OrderNumber,
                    StatusId = column.StatusId
                });
            }

            foreach (var statusModel in board.Status)
            {
                result.Status.Add(new StatusModel()
                {
                    Id = ObjectId.Parse(statusModel.Id),
                    Name = statusModel.Name,
                    Description = statusModel.Description,
                    Status = (Models.Enum.StatusEnum)statusModel.Status
                });
            }

            foreach (var update in board.Updates)
            {
                result.Updates.Add(new UpdateModel()
                {
                    Id = ObjectId.Parse(update.Id),
                    Description = update.Description,
                    Name = update.Name,
                    TimeStamp = update.TimeStamp,
                    ByUserId = update.ByUserId
                });
            }

            foreach (var user in board.Users)
            {
                result.Users.Add(new UserModel()
                {
                    Id = user.Id,
                    Role = (Models.Enum.BoardRoleEnum)user.Role,
                    Nickname = user.Nickname,
                    TeamRole = user.TeamRole
                });
            }

            return result;
        }

        public static SmallBoardDTO ToSmallBoardDTO(BoardModel model)
        {
            SmallBoardDTO result = new SmallBoardDTO()
            {
                Id = model.Id.ToString(),
                Name = model.Name,
                Description = model.Description,
            };
            return result;
        }

    }
}
