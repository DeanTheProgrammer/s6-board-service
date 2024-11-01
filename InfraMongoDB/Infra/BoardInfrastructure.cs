using BoardService.Interface;
using Models.Enum;
using Models.Models;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using CustomExceptions.ObjectExceptions;
using DTO;
using DTO.DTO_s.Board;
using InfraMongoDB.Transform;
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

        public Task AddUserToBoard(string BoardId, UserDTO User)
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
            NewBoard.SprintTime = SprintTimeEnum.None;
            NewBoard.Users = new List<UserModel>();
            NewBoard.Users.Add(new UserModel() { Id = UserId, Role = BoardRoleEnum.Admin });

            NewBoard.IsDeleted = false;

            await _boardCollection.InsertOneAsync(NewBoard);
            return NewBoard.Id.ToString();
        }

        public async Task DeleteBoard(string UserId, string BoardId)
        {
            BoardModel board = await _boardCollection.Find(b => b.Id == ObjectId.Parse(BoardId)).FirstOrDefaultAsync();
            if (board == null)
            {
                throw new NotFoundException("Board is not found");
            }

            board.IsDeleted = true;
            board.DeletedAt = DateTime.Now;
            board.DeletedBy = UserId;

            await _boardCollection.ReplaceOneAsync(b => b.Id == ObjectId.Parse(BoardId), board);
        }

        public async Task<List<BoardDTO>> GetActiveBoards(string UserId)
        {
            List<BoardModel> boards = await _boardCollection.Find(b => b.Users.Any(u => u.Id == UserId) && b.IsDeleted == false).ToListAsync();

            List<BoardDTO> result = new List<BoardDTO>();
            foreach (var board in boards)
            {
                result.Add(BoardTransform.ToDTO(board));
            }
            return result;
        }

        public async Task<BoardDTO> GetBoard(string BoardId, string UserId)
        {
            BoardModel result = await _boardCollection.Find(b => b.Id == ObjectId.Parse(BoardId) && b.Users.Any(u => u.Id == UserId)).FirstOrDefaultAsync();
            return Transform.BoardTransform.ToDTO(result);
        }

        public async Task<List<BoardDTO>> GetBoardArchived(string UserId)
        {
            List<BoardModel> boards = await _boardCollection.Find(b =>
                b.Users.Any(u => u.Id == UserId) && b.DeletedAt.Value.AddDays(30) > DateTime.Now).ToListAsync();

            List<BoardDTO> result = new List<BoardDTO>();
            foreach (BoardModel board in boards)
            {
                result.Add(Transform.BoardTransform.ToDTO(board));
            }
            return result;
        }

        public async Task<DateTime> GetBoardDeletedDate(string BoardId)
        {
            BoardModel board = await _boardCollection.Find(b => b.Id == ObjectId.Parse(BoardId) && b.IsDeleted == true)
                .FirstOrDefaultAsync();

            if (board.DeletedAt.Value == null)
            {
                throw new Exception("This should not be possible");
            }
            return board.DeletedAt.Value;
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

        public async Task RemoveUserFromBoard(string BoardId, string UserId)
        {
            BoardModel Board = await _boardCollection.Find(b => b.Id == ObjectId.Parse(BoardId)).FirstOrDefaultAsync();
            UserModel user = Board.Users.Find(u => u.Id == UserId);

            if (user == null)
            {
                throw new NotFoundException("User doesn't exist in board");
            }

            Board.Users.Remove(user);

            await _boardCollection.ReplaceOneAsync(b => b.Id == ObjectId.Parse(BoardId), Board);

        }

        public async Task UnDeleteBoard(string BoardId)
        {
            BoardModel Board = await _boardCollection.Find(b => b.Id == ObjectId.Parse(BoardId)).FirstOrDefaultAsync();

            Board.DeletedAt = DateTime.MinValue;
            Board.DeletedBy = null;
            Board.IsDeleted = false;

            await _boardCollection.ReplaceOneAsync(b => b.Id == ObjectId.Parse(BoardId), Board);
        }

        public async Task UpdateBoard(string BoardId, UpdateBoardDTO Board)
        {
            BoardModel OldBoard = await _boardCollection.Find(b => b.Id == ObjectId.Parse(BoardId)).FirstOrDefaultAsync();
            BoardModel NewBoard = Transform.BoardTransform.ToModelUpdate(OldBoard, Board);

            await _boardCollection.ReplaceOneAsync(b => b.Id == ObjectId.Parse(BoardId), NewBoard);

        }

        public async Task UpdateUserRole(string BoardId, string UserId, DTO.Enum.BoardRoleEnum Role)
        {
            BoardModel board = await _boardCollection.Find(b => b.Id == ObjectId.Parse(BoardId)).FirstOrDefaultAsync();
            if (board == null)
            {
                throw new NotFoundException("Board not found");
            }

            foreach (UserModel user in board.Users)
            {
                if (user.Id == UserId)
                {
                    user.Role = (BoardRoleEnum)Role;
                }
            }

            await _boardCollection.ReplaceOneAsync(b => b.Id == ObjectId.Parse(BoardId), board);

        }
    }
}
