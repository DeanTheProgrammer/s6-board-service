using System;
using DTO;
using DTO.DTO_s.InviteLink;

namespace BoardService.Interface;

public interface InviteDSInterface
{
    public Task<string> CreateInviteLink(InviteLinkDTO Invite);
    public Task<InviteLinkDTO> GetInviteLinkByCode(string Code);
    public Task<InviteLinkDTO> GetInviteLinkById(string Id);
    public Task<List<InviteLinkDTO>> GetInviteLinkByBoardId(string BoardId);
    public Task<List<OpenInviteLinkDTO>> GetPublicInviteLinkByBoardId(string BoardId);
    public Task<OpenInviteLinkDTO> GetPublicInviteLinkById(string id);
    public Task<string> UpdateInviteLink(InviteLinkDTO Invite);
    public Task DeleteInviteLink(string InviteLinkId);
}