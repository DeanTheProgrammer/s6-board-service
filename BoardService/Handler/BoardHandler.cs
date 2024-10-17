using System.ComponentModel.DataAnnotations;
using BoardService.Interface;
using CustomExceptions.ObjectExceptions;
using DTO;
using DTO.DTO_s.Board;
using DTO.DTO_s.InviteLink;
using DTO.Enum;
using Microsoft.Extensions.Logging;

namespace BoardService.Handler
{
    public class BoardHandler
    {
        private readonly BoardDSInterface _boardDsInterface;
        private readonly InviteDSInterface _InviteDsInterface;
        private readonly ILogger<BoardHandler> _logger;

        public BoardHandler(BoardDSInterface boardDsInterface, InviteDSInterface? InviteDsInterface, ILogger<BoardHandler>? logger)
        {
            _boardDsInterface = boardDsInterface;
            _InviteDsInterface = InviteDsInterface; 
            _logger = logger;
        }

        public async Task<BoardDTO> CreateBoard(string UserId, CreateBoardDTO Board)
        {
            if (String.IsNullOrEmpty(Board.Name))
            {
                throw new ValidationException("Name cannot be null");
            }

            string Id = await _boardDsInterface.CreateBoard(UserId, Board);
            BoardDTO result = await _boardDsInterface.GetBoard(Id, UserId);

            return result;
        }

        public async Task<BoardDTO> GetBoard(string BoardId, string UserId)
        {
            return await _boardDsInterface.GetBoard(BoardId, UserId);
        }

        public async Task<List<BoardDTO>> GetBoards(string UserId)
        {
            return await _boardDsInterface.GetBoards(UserId);
        }

        public async Task<SmallBoardDTO> GetSmallBoard(string BoardId, string UserId)
        {
            return await _boardDsInterface.GetSmallBoard(BoardId, UserId);
        }

        public async Task<List<SmallBoardDTO>> GetSmallBoards(string UserId)
        {
            return await _boardDsInterface.GetSmallBoards(UserId);
        }

        public async Task<InviteLinkDTO> CreateInviteLink(string UserId, CreateInviteLinkDTO inviteLink)
        {

            if(string.IsNullOrEmpty(inviteLink.BoardId))
            {
                throw new ValidationException("BoardId cannot be null");
            }

            BoardDTO Board = await _boardDsInterface.GetBoard(inviteLink.BoardId, UserId);
            if(Board == null)
            {
                throw new NotFoundException("Board not found");
            }

            var temp = Board.Users.Find(u => u.Id == UserId && u.Role == BoardRoleEnum.Admin);
            if (temp == null)
            {
                throw new UnauthorizedAccessException("You're not authorized to this");
            }

            if (inviteLink.ExpirationDate == null || inviteLink.ExpirationDate == DateTime.MinValue)
            {
                inviteLink.ExpirationDate = DateTime.Now.AddDays(7);
            }

            string LinkCode = Guid.NewGuid() +"-"+ Guid.NewGuid();
            InviteLinkDTO NewInvite = new InviteLinkDTO()
            {
                BoardId = inviteLink.BoardId,
                ExpiresAt = inviteLink.ExpirationDate.GetValueOrDefault(),
                LinkCode = LinkCode,
                ReceivingRole = inviteLink.BoardRole,
                CreatedBy = UserId,
                CreatedAt = DateTime.Now
            };

            string id = await _InviteDsInterface.CreateInviteLink(NewInvite);
            InviteLinkDTO result = await _InviteDsInterface.GetInviteLinkById(id);
            return result;
        }

    }
}
