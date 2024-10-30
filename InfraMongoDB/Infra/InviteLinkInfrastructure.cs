using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardService.Interface;
using DTO.DTO_s.InviteLink;
using InfraMongoDB.Transform;
using Microsoft.Extensions.Options;
using Models.Enum;
using Models.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InfraMongoDB.Infra
{
    public class InviteLinkInfrastructure : InviteDSInterface
    {
        private readonly IMongoCollection<InviteLinkModel> _InviteLinkCollection;
        public InviteLinkInfrastructure(IOptions<MongoDBSettings> options)
        {
            MongoDBSettings Settings = options.Value;

            if (Settings == null)
            {
                throw new Exception("The settings are null");
            }
            MongoClient client = new MongoClient(Settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(Settings.DatabaseName);
            _InviteLinkCollection = database.GetCollection<InviteLinkModel>("InviteLink");
        }

        public async Task<string> CreateInviteLink(InviteLinkDTO Invite)
        {
            Invite.Id = null;

            InviteLinkModel NewInvite = new InviteLinkModel()
            {
                BoardId = Invite.BoardId,
                LinkCode = Invite.LinkCode,
                CreatedBy = Invite.CreatedBy,
                ReceivingRole = (BoardRoleEnum)Invite.ReceivingRole,
                CreatedAt = Invite.CreatedAt,
                ExpiresAt = Invite.ExpiresAt
            };

            await _InviteLinkCollection.InsertOneAsync(NewInvite);

            return NewInvite.Id.ToString();
        }

        public Task DeleteInviteLink(string Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<InviteLinkDTO>> GetInviteLinkByBoardId(string BoardId)
        {
            List<InviteLinkDTO> result = new List<InviteLinkDTO>();

            List<InviteLinkModel> inviteLinks = await _InviteLinkCollection.Find(i => i.BoardId == BoardId).ToListAsync();

            foreach (InviteLinkModel invite in inviteLinks)
            {
                result.Add(InviteLinkTransform.ToDTO(invite));
            }
            return result;
        }

        public async Task<InviteLinkDTO> GetInviteLinkByCode(string Code)
        {
            InviteLinkModel inviteLink = await _InviteLinkCollection.Find(i => i.LinkCode == Code).FirstOrDefaultAsync();
            return Transform.InviteLinkTransform.ToDTO(inviteLink);
        }

        public async Task<InviteLinkDTO> GetInviteLinkById(string Id)
        {
            InviteLinkModel inviteLink = await _InviteLinkCollection.Find(i => i.Id == ObjectId.Parse(Id)).FirstOrDefaultAsync();

            return Transform.InviteLinkTransform.ToDTO(inviteLink);
        }

        public async Task<List<OpenInviteLinkDTO>> GetPublicInviteLinkByBoardId(string BoardId)
        {
            List<InviteLinkModel> inviteLinks =
                await _InviteLinkCollection.Find(i => i.BoardId == BoardId).ToListAsync();

            List<OpenInviteLinkDTO> result = new List<OpenInviteLinkDTO>();
            foreach (InviteLinkModel inviteLink in inviteLinks)
            {
                result.Add(Transform.InviteLinkTransform.ToOpenInviteLink(inviteLink));
            }

            return result;
        }

        public async Task<OpenInviteLinkDTO> GetPublicInviteLinkById(string id)
        {
            InviteLinkModel inviteLinkModel = await _InviteLinkCollection.FindAsync(i => i.Id == ObjectId.Parse(id)).Result.FirstOrDefaultAsync();

            return Transform.InviteLinkTransform.ToOpenInviteLink(inviteLinkModel);
        }

        public Task<string> UpdateInviteLink(InviteLinkDTO Invite)
        {
            throw new NotImplementedException();
        }
    }
}
