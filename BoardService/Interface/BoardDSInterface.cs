using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enum;
using Models.Models;

namespace BoardService.Interface
{
    public interface BoardDSInterface
    {
        public void AddUserToBoard(string BoardId, UserModel User);
        public void RemoveUserFromBoard(string BoardId, string UserId);
        public void UpdateUserRole(string BoardId, string UserId, BoardRoleEnum Role);
        public Task<string> CreateBoard(string UserId, string Name, string? Description);
        public string UpdateBoard(string UserId, BoardModel Board);
        public void DeleteBoard(string UserId, string BoardId);
        public List<BoardModel> GetActiveBoards(string UserId);
        public List<BoardModel> GetBoardArchived(string UserId);
        public DateTime GetBoardDeletedDate(string BoardId);
        public Task<BoardModel> GetBoard(string BoardId);
    }
}
