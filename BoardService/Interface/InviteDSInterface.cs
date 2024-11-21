using System;
using DTO;
using DTO.DTO_s.InviteLink;

namespace ProjectService.Interface;

public interface InviteDSInterface
{
    public Task<string> CreateInviteLink(InviteLinkDTO Invite);
    public Task<InviteLinkDTO> GetInviteLinkByCode(string Code);
    public Task<InviteLinkDTO> GetInviteLinkById(string Id);
    public Task<List<InviteLinkDTO>> GetInviteLinkByProjectId(string ProjectId);
    public Task<List<OpenInviteLinkDTO>> GetPublicInviteLinkByProjectId(string ProjectId);
    public Task<OpenInviteLinkDTO> GetPublicInviteLinkById(string id);
    public Task<string> UpdateInviteLink(InviteLinkDTO Invite);
    public Task DeleteInviteLink(string InviteLinkId);
}