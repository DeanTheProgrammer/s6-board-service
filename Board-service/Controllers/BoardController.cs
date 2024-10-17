using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using BoardService.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BoardService.Handler;
using DTO;
using DTO.DTO_s.Board;
using DTO.DTO_s.InviteLink;

namespace Board_service.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BoardController : ControllerBase
    {
        private readonly BoardHandler _board;
        private readonly string _userId = "TestUserId";
        private readonly ILogger<BoardController> _logger;
        private readonly InviteLinkHandler _inviteLinkHandler;

        public BoardController(BoardHandler board, ILogger<BoardController> logger)
        {
            _board = board;
            _logger = logger;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(BoardDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<BoardDTO> CreateBoard(CreateBoardDTO Board)
        {
            if (string.IsNullOrEmpty(Board.Name))
            {
                throw new ValidationException("Name cannot be null");
            }

            var Result = await _board.CreateBoard(_userId, Board);
            return Result;
        }

        [HttpGet("get")]
        [ProducesResponseType(typeof(BoardDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<BoardDTO> GetBoard(string BoardId)
        {
            var Result = await _board.GetBoard(BoardId, _userId);
            return Result;
        }

        [HttpGet("getall")]
        [ProducesResponseType(typeof(List<BoardDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<BoardDTO>> GetBoards()
        {
            var Result = await _board.GetBoards(_userId);
            return Result;
        }

        [HttpGet("getsmall")]
        [ProducesResponseType(typeof(SmallBoardDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<SmallBoardDTO> GetSmallBoard(string BoardId)
        {
            var Result = await _board.GetSmallBoard(BoardId, _userId);
            return Result;
        }

        [HttpGet("getallsmall")]
        [ProducesResponseType(typeof(List<SmallBoardDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<SmallBoardDTO>> GetSmallBoards()
        {
            var Result = await _board.GetSmallBoards(_userId);
            return Result;
        }


        [HttpPost("CreateInvite")]
        [ProducesResponseType(typeof(InviteLinkDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<InviteLinkDTO> CreateInviteLink(CreateInviteLinkDTO inviteLink)
        {
            return await _board.CreateInviteLink(_userId, inviteLink);
        }
    }
}
