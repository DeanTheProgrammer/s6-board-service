using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BoardService.Interface;
using CustomExceptions.ObjectExceptions;
using DTO.DTO_s.InviteLink;
using Microsoft.Extensions.Logging;

namespace BoardService.Handler
{
    public class InviteLinkHandler
    {
        private readonly InviteDSInterface _inviteLinkInterface;
        private readonly ILogger<InviteLinkHandler> _logger;

        public InviteLinkHandler(InviteDSInterface InviteLink, ILogger<InviteLinkHandler> logger)
        {
            _inviteLinkInterface = InviteLink;
            _logger = logger;
        }

        public async Task<InviteLinkDTO> GetLinkByCode(string Code)
        {
            InviteLinkDTO  Result = await _inviteLinkInterface.GetInviteLinkByCode(Code);
            if(Result == null)
            {
                throw new NotFoundException("InviteLink is not found");
            }
            return Result;
        }
    }
}
