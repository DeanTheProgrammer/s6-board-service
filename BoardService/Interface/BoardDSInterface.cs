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
        public void AddUserToBoard(string BoardId, UserDTO User);
        public void RemoveUserFromBoard(string BoardId, string UserId);
        public void UpdateUserRole(string BoardId, string UserId, BoardRoleEnum Role);
        public Task<string> CreateBoard(string UserId, CreateBoardDTO Board);
        public string UpdateBoard(string UserId, BoardDTO Board);
        public void DeleteBoard(string UserId, string BoardId);
        public List<BoardDTO> GetActiveBoards(string UserId);
        public List<BoardDTO> GetBoardArchived(string UserId);
        public DateTime GetBoardDeletedDate(string BoardId);
        public Task<BoardDTO> GetBoard(string BoardId, string UserId);
        public Task<List<BoardDTO>> GetBoards(string UserId);
        public Task<SmallBoardDTO> GetSmallBoard(string BoardId, string UserId);
        public Task<List<SmallBoardDTO>> GetSmallBoards(string UserId);
    }
}
