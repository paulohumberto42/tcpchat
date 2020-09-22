using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TcpChat.Server.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace TcpChat.Tests.Server.Services
{
    public class UserServiceTests
    {
        [Fact]
        public void AddingUser_WhenNameIsValidAndUsernameNotAlreadyExists_ShouldAddUser()
        {
            // Arrange
            var userService = this.CreateUserService();
            string username = "User001";

            // Act
            bool result = userService.TryAddUser(username, out _);

            // Assert
            Assert.True(result);
            Assert.True(userService.TryGetUserByName(username, out _));
        }

        [Fact]
        public void AddingUser_WhenNameAlreadyExists_ShouldNotAddUser()
        {
            // Arrange
            var userService = this.CreateUserService();
            string username = "User001";

            // Act
            userService.TryAddUser(username, out _);
            bool result = userService.TryAddUser(username, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(errorMessage, UserService.ErrorMessageUsernameAlreadyExists);
        }

        [Theory]
        [InlineData(" Username")]
        [InlineData("_Username")]
        [InlineData("3Username")]
        [InlineData("User!Name")]
        [InlineData("Username ")]
        [InlineData("")]
        [InlineData(null)]
        public void ValidatingUser_WhenNameIsInvalid_ShouldReturnFalse(string username)
        {
            // Arrange
            var userService = this.CreateUserService();

            // Act
            bool result = userService.ValidateUsername(username, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(errorMessage, UserService.ErrorMessageInvalidUsername);
        }

        [Fact]
        public void RemovingUser_WhenUserExists_UserShouldBeRemoved()
        {
            // Arrange
            string username = "User001";
            var userService = this.CreateUserService();
            userService.TryAddUser(username, out _);

            // Act
            bool result = userService.TryRemoveUser(username);

            // Assert
            Assert.True(result);
            Assert.False(userService.TryGetUserByName(username, out _));
        }

        [Fact]
        public void RemovingUser_WhenUserNotExists_ShouldReturnFalse()
        {
            // Arrange
            string existingUsername = "User001";
            string username = "User002";

            var userService = this.CreateUserService();
            userService.TryAddUser(existingUsername, out _);

            // Act
            bool result = userService.TryRemoveUser(username);

            // Assert
            Assert.False(result);
            Assert.True(userService.TryGetUserByName(existingUsername, out _));
        }

        [Fact]
        public void ListingUsers_WhenUsersExist_ShouldReturnAllUsers()
        {
            // Arrange
            string[] usernames = new string[]
            {
                "User001",
                "User002",
                "User003",
            };

            var userService = this.CreateUserService();

            foreach (string user in usernames)
            {
                userService.TryAddUser(user, out _);
            }


            // Act
            var users = userService.GetConnectedUsers();

            // Assert
            Assert.Equal(usernames, users.Select(p => p.Username).OrderBy(p => p));
        }


        private UserService CreateUserService()
        {
            var logger = Substitute.For<ILogger<UserService>>();
            return new UserService(logger);
        }
    }
}
