using BoardService.Handler;
using DTO.DTO_s.InviteLink;
using Microsoft.AspNetCore.Mvc;

namespace Board_service.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class InviteLinkController : Controller
    {
        private readonly InviteLinkHandler _handler;
        private readonly string _userId = "TestUserId";
        private readonly ILogger<InviteLinkController> _logger;

        public InviteLinkController(InviteLinkHandler handler, ILogger<InviteLinkController> logger)
        {
            _handler = handler;
            _logger = logger;
        }
        [HttpGet]
        public async Task<PublicInviteLinkDTO> GetInvite(string Code)
        {
            PublicInviteLinkDTO result = await _handler.GetLinkByCode(Code);

            return result;
        }
        [HttpPost]
        public async Task<InviteLinkDTO> Create([FromBody]CreateInviteLinkDTO dto)
        {
            InviteLinkDTO result = await _handler.CreateInviteLink(_userId, dto);
            return result;
        }
    }
}
