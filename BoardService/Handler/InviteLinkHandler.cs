using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ProjectService.Interface;
using CustomExceptions.ObjectExceptions;
using DTO.DTO_s.Project;
using DTO.DTO_s.InviteLink;
using DTO.Enum;
using Microsoft.Extensions.Logging;

namespace ProjectService.Handler
{
    public class InviteLinkHandler
    {
        private readonly InviteDSInterface _inviteLinkDSInterface;
        private readonly ILogger<InviteLinkHandler> _logger;
        private readonly ProjectHandler _ProjectHandler;

        public InviteLinkHandler(InviteDSInterface InviteLink, ILogger<InviteLinkHandler>? logger, ProjectHandler ProjectHandler)
        {
            this._inviteLinkDSInterface = InviteLink;
            _logger = logger;
            this._ProjectHandler = ProjectHandler;
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


            SmallProjectDTO ProjectDTO = await _ProjectHandler.GetSmallProject(inviteObject.ProjectId);

            publicInvite.ProjectName = ProjectDTO.Name;
            publicInvite.ProjectDescription = ProjectDTO.Description;

            return publicInvite;
        }

        public async Task<InviteLinkDTO> CreateInviteLink(string UserId, CreateInviteLinkDTO inviteLink)
        {
            if (string.IsNullOrEmpty(inviteLink.ProjectId))
            {
                throw new ValidationException("ProjectId cannot be null");
            }

            ProjectDTO Project = await _ProjectHandler.GetProject(inviteLink.ProjectId, UserId);
            if (Project == null)
            {
                throw new NotFoundException("Project not found");
            }

            var temp = Project.Users.Find(u => u.Id == UserId && u.Role == ProjectRoleEnum.Admin);
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
                ProjectId = inviteLink.ProjectId,
                ExpiresAt = inviteLink.ExpirationDate.GetValueOrDefault(),
                LinkCode = LinkCode,
                ReceivingRole = inviteLink.ProjectRole,
                CreatedBy = UserId,
                CreatedAt = DateTime.Now
            };

            string id = await _inviteLinkDSInterface.CreateInviteLink(NewInvite);
            InviteLinkDTO result = await _inviteLinkDSInterface.GetInviteLinkById(id);
            return result;
        }

        
    }
}
