using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using DTO;
using DTO.DTO_s.Project;
using DTO.DTO_s.InviteLink;
using ProjectService.Handler;

namespace Board_service.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectHandler _board;
        private readonly string _userId = "TestUserId";
        private readonly ILogger<ProjectController> _logger;
        private readonly InviteLinkHandler _inviteLinkHandler;

        public ProjectController(ProjectHandler board, ILogger<ProjectController> logger)
        {
            _board = board;
            _logger = logger;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ProjectDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ProjectDTO> CreateProject(CreateProjectDTO Board)
        {
            if (string.IsNullOrEmpty(Board.Name))
            {
                throw new ValidationException("Name cannot be null");
            }

            var Result = await _board.CreateProject(_userId, Board);
            return Result;
        }

        [HttpGet("get")]
        [ProducesResponseType(typeof(ProjectDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ProjectDTO> GetProject(string BoardId)
        {
            var Result = await _board.GetProject(BoardId, _userId);
            return Result;
        }

        [HttpGet("getall")]
        [ProducesResponseType(typeof(List<ProjectDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<ProjectDTO>> GetProjects()
        {
            var Result = await _board.GetProjects(_userId);
            return Result;
        }

        [HttpGet("getsmall")]
        [ProducesResponseType(typeof(SmallProjectDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<SmallProjectDTO> GetSmallProject(string BoardId)
        {
            var Result = await _board.GetSmallProject(BoardId);
            return Result;
        }

        [HttpGet("getallsmall")]
        [ProducesResponseType(typeof(List<SmallProjectDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<SmallProjectDTO>> GetSmallProjects()
        {
            var Result = await _board.GetSmallProjects(_userId);
            return Result;
        }
    }
}
