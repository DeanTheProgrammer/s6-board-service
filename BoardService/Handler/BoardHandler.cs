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

            string Id = await _boardDsInterface.CreateBoard(UserId, Board);
            BoardDTO result = await _boardDsInterface.GetBoard(Id, UserId);

            return result;
        }

        public async Task<BoardDTO> GetBoard(string BoardId, string UserId)
        {
            return await _boardDsInterface.GetBoard(BoardId, UserId);
        }

        public async Task<List<BoardDTO>> GetBoards(string UserId)
        {
            return await _boardDsInterface.GetBoards(UserId);
        }

        public async Task<SmallBoardDTO> GetSmallBoard(string BoardId, string UserId)
        {
            return await _boardDsInterface.GetSmallBoard(BoardId, UserId);
        }

        public async Task<List<SmallBoardDTO>> GetSmallBoards(string UserId)
        {
            return await _boardDsInterface.GetSmallBoards(UserId);
        }

    }
}
