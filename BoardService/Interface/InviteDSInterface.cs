using System;
using Models.Models;

namespace BoardService.Interface;

public interface InviteDSInterface
{
    public string CreateInviteLink(InviteLinkModel Invite);
    public InviteLinkModel GetInviteLinkByCode(string Code);
    public InviteLinkModel GetInviteLinkById(string InviteLinkId);
    public List<InviteLinkModel> GetInviteLinkByBoardId(string BoardId);
    public string UpdateInviteLink(InviteLinkModel Invite);
    public void DeleteInviteLink(string InviteLinkId);
}