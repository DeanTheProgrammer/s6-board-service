using Board_service.Handler.AuthorizationHandler;
using DTO.DTO_s.InviteLink;
using Microsoft.AspNetCore.Mvc;
using ProjectService.Handler;
using System.ComponentModel.DataAnnotations;

namespace Board_service.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class InviteLinkController : Controller
    {
        private readonly InviteLinkHandler _handler;
        private readonly string _userId = "TestUserId";
        private readonly ILogger<InviteLinkController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InviteLinkController(InviteLinkHandler handler, ILogger<InviteLinkController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _handler = handler;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public async Task<PublicInviteLinkDTO> GetInvite(string Code)
        {
            var HttpContext = _httpContextAccessor.HttpContext;
            if (HttpContext != null)
            {

                var UserId = Auth0AuthorizationHandler.GetUserIdFromContext(HttpContext);
                if (!string.IsNullOrEmpty(UserId))
                {
                    PublicInviteLinkDTO result = await _handler.GetLinkByCode(Code);
                    return result;
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
        [HttpPost]
        public async Task<InviteLinkDTO> Create([FromBody]CreateInviteLinkDTO dto)
        {
            var HttpContext = _httpContextAccessor.HttpContext;
            if (HttpContext != null)
            {

                var UserId = Auth0AuthorizationHandler.GetUserIdFromContext(HttpContext);
                if (!string.IsNullOrEmpty(UserId))
                {
                    InviteLinkDTO result = await _handler.CreateInviteLink(_userId, dto);
                    return result;
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
