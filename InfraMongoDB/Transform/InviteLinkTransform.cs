using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.DTO_s.InviteLink;
using DTO.Enum;

namespace InfraMongoDB.Transform
{
    public class InviteLinkTransform
    {
        public static DTO.DTO_s.InviteLink.InviteLinkDTO ToDTO(Models.Models.InviteLinkModel model)
        {
            return new DTO.DTO_s.InviteLink.InviteLinkDTO()
            {
                Id = model.Id.ToString(),
                ProjectId = model.ProjectId,
                LinkCode = model.LinkCode,
                CreatedBy = model.CreatedBy,
                ReceivingRole = (ProjectRoleEnum)model.ReceivingRole,
                CreatedAt = model.CreatedAt,
                ExpiresAt = model.ExpiresAt
            };
        }

        public static Models.Models.InviteLinkModel ToModel(DTO.DTO_s.InviteLink.InviteLinkDTO dto)
        {
            return new Models.Models.InviteLinkModel()
            {
                Id = new MongoDB.Bson.ObjectId(dto.Id),
                ProjectId = dto.ProjectId,
                LinkCode = dto.LinkCode,
                CreatedBy = dto.CreatedBy,
                ReceivingRole = (Models.Enum.ProjectRoleEnum)dto.ReceivingRole,
                CreatedAt = dto.CreatedAt,
                ExpiresAt = dto.ExpiresAt
            };
        }

        public static DTO.DTO_s.InviteLink.OpenInviteLinkDTO ToOpenInviteLink(Models.Models.InviteLinkModel model)
        {
            return new DTO.DTO_s.InviteLink.OpenInviteLinkDTO()
            {
                Id = model.Id.ToString(),
                ProjectId = model.ProjectId,
                CreatedBy = model.CreatedBy,
                ReceivingRole = (ProjectRoleEnum)model.ReceivingRole,
                CreatedAt = model.CreatedAt,
                ExpiresAt = model.ExpiresAt
            };
        }
    }
}
