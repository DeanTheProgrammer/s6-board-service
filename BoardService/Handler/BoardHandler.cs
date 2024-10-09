using System.ComponentModel.DataAnnotations;
using BoardService.Interface;
using DTO;
using DTO.DTO_s.Board;
using DTO.Enum;
using Models.Enum;
using Models.Models;
using MongoDB.Bson;

namespace BoardService.Handler
{
    public class BoardHandler
    {
        private readonly BoardDSInterface _boardDsInterface;
        //private readonly InviteDSInterface _InviteDsInterface;

        public BoardHandler(BoardDSInterface boardDsInterface, InviteDSInterface? InviteDsInterface)
        {
            _boardDsInterface = boardDsInterface;
            //_InviteDsInterface = InviteDsInterface;
        }

        public async Task<BoardDTO> CreateBoard(string UserId, CreateBoardDTO Board)
        {
            if (String.IsNullOrEmpty(Board.Name))
            {
                throw new ValidationException("Name cannot be null");
            }

            string Id = await _boardDsInterface.CreateBoard(UserId, Board.Name, Board.Description);
            BoardModel boardModel = await _boardDsInterface.GetBoard(Id);

            return MapBoardModelToDTO(boardModel);
        }



        private BoardDTO MapBoardModelToDTO(BoardModel boardModel)
        {
            BoardDTO boardDTO = new BoardDTO()
            {
                Id = boardModel.Id.ToString(),
                Name = boardModel.Name,
                Description = boardModel.Description,
                CreatedBy = boardModel.CreatedBy,
                CreatedAt = boardModel.CreatedAt,
                Updates = MapUpdateModelToDTO(boardModel.Updates),
                Users = MapUserModelToDTO(boardModel.Users),
                Columns = MapColumnsModelToDTO(boardModel.Columns),
                Status = MapStatusModelToDTO(boardModel.Status)
            };

            return boardDTO;
        }

        private List<UpdateDTO> MapUpdateModelToDTO(List<UpdateModel> updateModels)
        {
            List<UpdateDTO> updateDTOs = new List<UpdateDTO>();

            foreach (var update in updateModels)
            {
                UpdateDTO updateDTO = new UpdateDTO()
                {
                    Id = update.Id.ToString(),
                    Description = update.Description,
                    Name = update.Name,
                    TimeStamp = update.TimeStamp,
                    ByUserId = update.ByUserId
                };

                updateDTOs.Add(updateDTO);
            }

            return updateDTOs;
        }
        private List<UserDTO> MapUserModelToDTO(List<UserModel> userModels)
        {
            List<UserDTO> userDTOs = new List<UserDTO>();

            foreach (var user in userModels)
            {
                UserDTO userDTO = new UserDTO()
                {
                    Id = user.Id.ToString(),
                    Role = (DTO.Enum.BoardRoleEnum)user.Role,
                    Nickname = user.Nickname,
                    TeamRole = user.TeamRole
                };

                userDTOs.Add(userDTO);
            }

            return userDTOs;
        }

        private List<ColumnsDTO> MapColumnsModelToDTO(List<ColumnsModel> columnsModels)
        {
            List<ColumnsDTO> columnsDTOs = new List<ColumnsDTO>();

            foreach (var column in columnsModels)
            {
                ColumnsDTO columnsDTO = new ColumnsDTO()
                {
                    Id = column.Id.ToString(),
                    Name = column.Name,
                    Description = column.Description,
                    OrderNumber = column.OrderNumber,
                    StatusId = column.StatusId.ToString()
                };

                columnsDTOs.Add(columnsDTO);
            }

            return columnsDTOs;
        }
        private List<StatusDTO> MapStatusModelToDTO(List<StatusModel> statusModels)
        {
            List<StatusDTO> statusDTOs = new List<StatusDTO>();

            foreach (var status in statusModels)
            {
                StatusDTO statusDTO = new StatusDTO()
                {
                    Id = status.Id.ToString(),
                    Name = status.Name,
                    Description = status.Description,
                    Status = (DTO.Enum.StatusEnum)status.Status
                };

                statusDTOs.Add(statusDTO);
            }

            return statusDTOs;
        }
    }
}
