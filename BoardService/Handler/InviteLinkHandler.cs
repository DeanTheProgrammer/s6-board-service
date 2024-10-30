using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BoardService.Interface;
using CustomExceptions.ObjectExceptions;
using DTO.DTO_s.Board;
using DTO.DTO_s.InviteLink;
using DTO.Enum;
using Microsoft.Extensions.Logging;

namespace BoardService.Handler
{
    public class InviteLinkHandler
    {
        private readonly InviteDSInterface _inviteLinkDSInterface;
        private readonly ILogger<InviteLinkHandler> _logger;
        private readonly BoardHandler _boardHandler;

        public InviteLinkHandler(InviteDSInterface InviteLink, ILogger<InviteLinkHandler>? logger, BoardHandler boardHandler)
        {
            this._inviteLinkDSInterface = InviteLink;
            _logger = logger;
            this._boardHandler = boardHandler;
        }

        public async Task<PublicInviteLinkDTO> GetLinkByCode(string Code)
        {
            InviteLinkDTO  inviteObject = await _inviteLinkDSInterface.GetInviteLinkByCode(Code);
            if(inviteObject == null)
            {
                throw new NotFoundException("InviteLink is not found");
            }

            PublicInviteLinkDTO publicInvite = new PublicInviteLinkDTO()
            {
                ExpiresAt = inviteObject.ExpiresAt,
                LinkCode = inviteObject.LinkCode,
                ReceivingRole = inviteObject.ReceivingRole,
                Code = Code
            };


            SmallBoardDTO BoardDTO = await _boardHandler.GetSmallBoard(inviteObject.BoardId);

            publicInvite.BoardName = BoardDTO.Name;
            publicInvite.BoardDescription = BoardDTO.Description;

            return publicInvite;
        }

        public async Task<InviteLinkDTO> CreateInviteLink(string UserId, CreateInviteLinkDTO inviteLink)
        {
            if (string.IsNullOrEmpty(inviteLink.BoardId))
            {
                throw new ValidationException("BoardId cannot be null");
            }

            BoardDTO board = await _boardHandler.GetBoard(inviteLink.BoardId, UserId);
            if (board == null)
            {
                throw new NotFoundException("Board not found");
            }

            var temp = board.Users.Find(u => u.Id == UserId && u.Role == BoardRoleEnum.Admin);
            if (temp == null)
            {
                throw new UnauthorizedAccessException("You're not authorized to this");
            }


            if (inviteLink.ExpirationDate == null || inviteLink.ExpirationDate == DateTime.MinValue)
            {
                inviteLink.ExpirationDate = DateTime.Now.AddDays(7);
            }

            string LinkCode = Guid.NewGuid() + "-" + Guid.NewGuid();
            InviteLinkDTO NewInvite = new InviteLinkDTO()
            {
                BoardId = inviteLink.BoardId,
                ExpiresAt = inviteLink.ExpirationDate.GetValueOrDefault(),
                LinkCode = LinkCode,
                ReceivingRole = inviteLink.BoardRole,
                CreatedBy = UserId,
                CreatedAt = DateTime.Now
            };

            string id = await _inviteLinkDSInterface.CreateInviteLink(NewInvite);
            InviteLinkDTO result = await _inviteLinkDSInterface.GetInviteLinkById(id);
            return result;
        }

        
    }
}
