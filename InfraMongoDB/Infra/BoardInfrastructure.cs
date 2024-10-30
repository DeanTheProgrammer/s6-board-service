using BoardService.Interface;
using Models.Enum;
using Models.Models;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using Amazon.Runtime.Internal;
using DTO;
using DTO.DTO_s.Board;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace InfraMongoDB.Infra
{
    public class BoardInfrastructure : BoardDSInterface
    {
        private readonly IMongoCollection<BoardModel> _boardCollection;

        public BoardInfrastructure(IOptions<MongoDBSettings> options)
        {

            MongoDBSettings Settings = options.Value;
            MongoClient client = new MongoClient(Settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(Settings.DatabaseName);
            _boardCollection = database.GetCollection<BoardModel>("Board");
        }

        public void AddUserToBoard(string BoardId, UserDTO User)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CreateBoard(string UserId, CreateBoardDTO input)
        {
            if (string.IsNullOrEmpty(input.Name))
            {
                throw new ValidationException("Name cannot be null");
            }

            BoardModel NewBoard = new BoardModel()
            {
                Name = input.Name,
            };

            if (!string.IsNullOrEmpty(input.Description))
            {
                NewBoard.Description = input.Description;
            }
            else
            {
                NewBoard.Description = "";
            }

            NewBoard.CreatedBy = UserId;
            NewBoard.CreatedAt = DateTime.Now;
            NewBoard.Updates = new List<UpdateModel>();
            NewBoard.Users = new List<UserModel>();
            NewBoard.Users.Add(new UserModel() { Id = UserId, Role = BoardRoleEnum.Admin });

            NewBoard.Status = new List<StatusModel>();

            ObjectId toDoObjectId = ObjectId.GenerateNewId();
            ObjectId inProgressObjectId = ObjectId.GenerateNewId();
            ObjectId doneObjectId = ObjectId.GenerateNewId();


            NewBoard.Status.Add(new StatusModel() { Id = toDoObjectId, Name = "To Do", Description = "Task is not started", Status = StatusEnum.ToDo });
            NewBoard.Status.Add(new StatusModel() { Id = inProgressObjectId, Name = "In Progress", Description = "Task is in progress", Status = StatusEnum.InProgress });
            NewBoard.Status.Add(new StatusModel() { Id = doneObjectId, Name = "Done", Description = "Task is completed", Status = StatusEnum.Done });

            NewBoard.Columns = new List<ColumnsModel>();
            NewBoard.Columns.Add(new ColumnsModel() { Name = "To Do", OrderNumber = 0, StatusId = toDoObjectId.ToString() });
            NewBoard.Columns.Add(new ColumnsModel() { Name = "In Progress", OrderNumber = 1, StatusId = inProgressObjectId.ToString() });
            NewBoard.Columns.Add(new ColumnsModel() { Name = "Done", OrderNumber = 2, StatusId = doneObjectId.ToString() });

            await _boardCollection.InsertOneAsync(NewBoard);
            return NewBoard.Id.ToString();
        }

        public void DeleteBoard(string UserId, string BoardId)
        {
            throw new NotImplementedException();
        }

        public List<BoardDTO> GetActiveBoards(string UserId)
        {
            throw new NotImplementedException();
        }

        public async Task<BoardDTO> GetBoard(string BoardId, string UserId)
        {
            BoardModel result = await _boardCollection.Find(b => b.Id == ObjectId.Parse(BoardId) && b.Users.Any(u => u.Id == UserId)).FirstOrDefaultAsync();
            return Transform.BoardTransform.ToDTO(result);
        }

        public List<BoardDTO> GetBoardArchived(string UserId)
        {
            throw new NotImplementedException();
        }

        public DateTime GetBoardDeletedDate(string BoardId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BoardDTO>> GetBoards(string UserId)
        { 
            List<BoardDTO> Result = new List<BoardDTO>();
            List<BoardModel> models = await _boardCollection.Find(b => b.Users.Any(u => u.Id == UserId)).ToListAsync();
            foreach (var model in models)
            {
                Result.Add(Transform.BoardTransform.ToDTO(model));
            }
            return Result;
        }

        public async Task<SmallBoardDTO> GetSmallBoard(string BoardId)
        {
            
            BoardModel model = await _boardCollection.Find(b => b.Id == ObjectId.Parse(BoardId)).FirstOrDefaultAsync();

            return Transform.BoardTransform.ToSmallBoardDTO(model);
        }

        public async Task<List<SmallBoardDTO>> GetSmallBoards(string UserId)
        {
            List<SmallBoardDTO> Result = new List<SmallBoardDTO>();
            List<BoardModel> models = await _boardCollection.Find(b => b.Users.Any(u => u.Id == UserId)).ToListAsync();
            foreach (BoardModel model in models)
            {
                Result.Add(Transform.BoardTransform.ToSmallBoardDTO(model));
            }

            return Result;
        }

        public void RemoveUserFromBoard(string BoardId, string UserId)
        {
            throw new NotImplementedException();
        }

        public string UpdateBoard(string UserId, BoardDTO Board)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserRole(string BoardId, string UserId, DTO.Enum.BoardRoleEnum Role)
        {
            throw new NotImplementedException();
        }
    }
}
