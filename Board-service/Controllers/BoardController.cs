using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using BoardService.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BoardService.Handler;

namespace Board_service.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BoardController : Controller
    {
        private readonly BoardHandler _board;
        public BoardController(BoardDSInterface boardDsInterface)
        {
            _board = new BoardHandler(boardDsInterface, null);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateBoard(string UserId, string Name, string? Description)
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Name))
            {
                throw new ValidationException("Name cannot be null");
            }
            
            var Result = await _board.CreateBoard(UserId, Name, Description);
            return Ok(Result);
        }
    }
}
