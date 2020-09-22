using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TcpChat.Networking.Server;
using TcpChat.Server.Model;
using TcpChat.Server.ServiceContracts;
using Microsoft.Extensions.Logging;

namespace TcpChat.Server.Services
{
    public class UserService : IUserService
    {
        public const string ErrorMessageInvalidUsername = "Invalid username";
        public const string ErrorMessageUsernameAlreadyExists = "Username already exists";

        private readonly ILogger logger;
        private readonly Regex usernameRegex;
        private readonly ConcurrentDictionary<string, ChatUser> users;

        public UserService(ILogger<UserService> logger)
        {
            this.logger = logger;
            this.usernameRegex = new Regex("^[A-Za-z][A-Za-z0-9_-]+$");
            this.users = new ConcurrentDictionary<string, ChatUser>();
        }

        public bool TryGetUserByName(string username, out ChatUser user)
        {
            return this.users.TryGetValue(username, out user);
        }

        public IEnumerable<ChatUser> GetConnectedUsers()
        {
            return this.users.Values;
        }

        public bool ValidateUsername(string username, out string errorMessage)
        {
            if (string.IsNullOrEmpty(username) || !this.usernameRegex.IsMatch(username))
            {
                errorMessage = ErrorMessageInvalidUsername;
                return false;
            }

            if (this.users.ContainsKey(username))
            {
                errorMessage = ErrorMessageUsernameAlreadyExists;
                return false;
            }

            errorMessage = null;
            return true;
        }

        public bool TryAddUser(string username, out string errorMessage)
        {
            if (this.ValidateUsername(username, out errorMessage))
            {
                this.users.GetOrAdd(username, new ChatUser(username));
                this.logger.LogDebug("Client connected - session: {SessionId}", username);
                return true;
            }

            return false;
        }

        public bool TryRemoveUser(string username)
        {
            if (this.users.TryRemove(username, out ChatUser user))
            {
                this.logger.LogDebug("Client disconnected - username: {username}", user.Username);
                return true;
            }

            return false;
        }
    }
}
