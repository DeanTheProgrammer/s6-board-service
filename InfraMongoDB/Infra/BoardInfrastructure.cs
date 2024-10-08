using BoardService.Interface;
using Models.Enum;
using Models.Models;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using MongoDB.Driver;

namespace InfraMongoDB.Infra
{
    public class BoardInfrastructure : BoardDSInterface
    {
        private readonly IMongoCollection<BoardModel> _boardCollection;

        public BoardInfrastructure(MongoDBSettings Settings)
        {
            MongoClient client = new MongoClient(Settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(Settings.DatabaseName);
            _boardCollection = database.GetCollection<BoardModel>("Board");
        }
        public void AddUserToBoard(string BoardId, UserModel User)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CreateBoard(string UserId, string Name, string? Description)
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new ValidationException("Name cannot be null");
            }

            BoardModel NewBoard = new BoardModel()
            {
                Name = Name,
            };

            if (!string.IsNullOrEmpty(Description))
            {
                NewBoard.Description = Description;
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

        public List<BoardModel> GetActiveBoards(string UserId)
        {
            throw new NotImplementedException();
        }

        public async Task<BoardModel> GetBoard(string BoardId)
        {
            BoardModel result = await _boardCollection.FindAsync(Builders<BoardModel>.Filter.Eq("Id", ObjectId.Parse(BoardId))).Result.FirstOrDefaultAsync();
            return result;
        }

        public List<BoardModel> GetBoardArchived(string UserId)
        {
            throw new NotImplementedException();
        }

        public DateTime GetBoardDeletedDate(string BoardId)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserFromBoard(string BoardId, string UserId)
        {
            throw new NotImplementedException();
        }

        public string UpdateBoard(string UserId, BoardModel Board)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserRole(string BoardId, string UserId, BoardRoleEnum Role)
        {
            throw new NotImplementedException();
        }
    }
}
