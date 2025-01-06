using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using Board_service.Handler.AuthorizationHandler;
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
        private readonly ILogger<ProjectController> _logger;
        private readonly InviteLinkHandler _inviteLinkHandler;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProjectController(ProjectHandler board, ILogger<ProjectController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _board = board;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ProjectDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ProjectDTO> CreateProject(CreateProjectDTO Board)
        {
            var HttpContext = _httpContextAccessor.HttpContext;
            if (HttpContext != null)
            {

                var UserId = Auth0AuthorizationHandler.GetUserIdFromContext(HttpContext);
                if (!string.IsNullOrEmpty(UserId))
                {
                    if (string.IsNullOrEmpty(Board.Name))
                    {
                        throw new ValidationException("Name cannot be null");
                    }

                    var Result = await _board.CreateProject(UserId, Board);
                    return Result;
                }
                else
                {
                    throw new UnauthorizedAccessException("You're not authorized");
                }
            }
            else
            {
                throw new ValidationException("HttpContext is null");
            }
        }

        [HttpGet("get")]
        [ProducesResponseType(typeof(ProjectDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ProjectDTO> GetProject(string BoardId)
        {
            var HttpContext = _httpContextAccessor.HttpContext;
            if (HttpContext != null)
            {

                var UserId = Auth0AuthorizationHandler.GetUserIdFromContext(HttpContext);
                if (!string.IsNullOrEmpty(UserId))
                {
                    var Result = await _board.GetProject(BoardId, UserId);
                    return Result;
                }
                else
                {
                    throw new UnauthorizedAccessException("You're not authorized");
                }
            }
            else
            {
                throw new ValidationException("HttpContext is null");
            }
        }

        [HttpGet("getall")]
        [ProducesResponseType(typeof(List<ProjectDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<ProjectDTO>> GetProjects()
        {
            var HttpContext = _httpContextAccessor.HttpContext;
            if (HttpContext != null)
            {

                var UserId = Auth0AuthorizationHandler.GetUserIdFromContext(HttpContext);
                if (!string.IsNullOrEmpty(UserId))
                {
                    var Result = await _board.GetProjects(UserId);
                    return Result;
                }
                else
                {
                    throw new UnauthorizedAccessException("You're not authorized");
                }
            }
            else
            {
                throw new ValidationException("HttpContext is null");
            }
        }

        [HttpGet("getsmall")]
        [ProducesResponseType(typeof(SmallProjectDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<SmallProjectDTO> GetSmallProject(string BoardId)
        {
            var HttpContext = _httpContextAccessor.HttpContext;
            if (HttpContext != null)
            {

                var UserId = Auth0AuthorizationHandler.GetUserIdFromContext(HttpContext);
                if (!string.IsNullOrEmpty(UserId))
                {
                    var Result = await _board.GetSmallProject(BoardId);
                    return Result;
                }
                else
                {
                    throw new UnauthorizedAccessException("You're not authorized");
                }
            }
            else
            {
                throw new ValidationException("HttpContext is null");
            }
        }

        [HttpGet("getallsmall")]
        [ProducesResponseType(typeof(List<SmallProjectDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<SmallProjectDTO>> GetSmallProjects()
        {
            var HttpContext = _httpContextAccessor.HttpContext;
            if (HttpContext != null)
            {

                var UserId = Auth0AuthorizationHandler.GetUserIdFromContext(HttpContext);
                if (!string.IsNullOrEmpty(UserId))
                {
                    var Result = await _board.GetSmallProjects(UserId);
                    return Result;
                }
                else
                {
                    throw new UnauthorizedAccessException("You're not authorized");
                }
            }
            else
            {
                throw new ValidationException("HttpContext is null");
            }
        }
    }
}
