using System.ComponentModel.DataAnnotations;
using BoardService.Interface;
using CustomExceptions.ObjectExceptions;
using DTO;
using DTO.DTO_s.Board;
using DTO.DTO_s.InviteLink;
using DTO.Enum;
using Microsoft.Extensions.Logging;

namespace BoardService.Handler
{
    public class BoardHandler
    {
        private readonly BoardDSInterface _boardDsInterface;
        private readonly ILogger<BoardHandler> _logger;

        public BoardHandler(BoardDSInterface boardDsInterface, ILogger<BoardHandler>? logger)
        {
            this._boardDsInterface = boardDsInterface;
            _logger = logger;
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

        public async Task<SmallBoardDTO> GetSmallBoard(string BoardId)
        {
            return await _boardDsInterface.GetSmallBoard(BoardId);
        }

        public async Task<List<SmallBoardDTO>> GetSmallBoards(string UserId)
        {
            return await _boardDsInterface.GetSmallBoards(UserId);
        }

    }
}
