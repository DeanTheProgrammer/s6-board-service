using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DTO.DTO_s.Board;
using DTO.Enum;


namespace BoardService.Interface
{
    public interface BoardDSInterface
    {
        public Task AddUserToBoard(string BoardId, UserDTO User);
        public Task RemoveUserFromBoard(string BoardId, string UserId);
        public Task UpdateUserRole(string BoardId, string UserId, BoardRoleEnum Role);
        public Task<string> CreateBoard(string UserId, CreateBoardDTO Board);
        public Task UpdateBoard(string BoardId, UpdateBoardDTO Board);
        public Task DeleteBoard(string UserId, string BoardId);
        public Task UnDeleteBoard(string BoardId);
        public Task<List<BoardDTO>> GetActiveBoards(string UserId);
        public Task<List<BoardDTO>> GetBoardArchived(string UserId);
        public Task<DateTime> GetBoardDeletedDate(string BoardId);
        public Task<BoardDTO> GetBoard(string BoardId, string UserId);
        public Task<List<BoardDTO>> GetBoards(string UserId);
        public Task<SmallBoardDTO> GetSmallBoard(string BoardId);
        public Task<List<SmallBoardDTO>> GetSmallBoards(string UserId);
    }
}
