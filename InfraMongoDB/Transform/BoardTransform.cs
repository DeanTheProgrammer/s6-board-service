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
using SprintTimeEnum = Models.Enum.SprintTimeEnum;

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
                SprintTime = (DTO.Enum.SprintTimeEnum)board.SprintTime,
                CreatedAt = board.CreatedAt,
                CreatedBy = board.CreatedBy,
                Users = new List<UserDTO>(),
                IsDeleted = board.IsDeleted,
                DeletedAt = board.DeletedAt
            };


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
                SprintTime = (SprintTimeEnum) board.SprintTime,
                CreatedAt = board.CreatedAt,
                CreatedBy = board.CreatedBy,
                Users = new List<UserModel>(),
                IsDeleted = board.IsDeleted,
                DeletedAt = board.DeletedAt
            };



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


        public static BoardModel ToModelUpdate(BoardModel Old, UpdateBoardDTO newBoard)
        {

            Old.Name = newBoard.Name;
            Old.Description = newBoard.Description;
            Old.SprintTime = (SprintTimeEnum)newBoard.SprintTime;

            return Old;
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
