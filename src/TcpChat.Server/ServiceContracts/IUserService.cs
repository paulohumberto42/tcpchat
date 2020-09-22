using System.Collections.Generic;
using TcpChat.Server.Model;

namespace TcpChat.Server.ServiceContracts
{
    public interface IUserService
    {
        bool TryGetUserByName(string username, out ChatUser user);

        bool ValidateUsername(string username, out string errorMessage);

        bool TryAddUser(string username, out string errorMessage);

        bool TryRemoveUser(string username);

        IEnumerable<ChatUser> GetConnectedUsers();
    }
}
