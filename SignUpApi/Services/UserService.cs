using ApiValidation.Models;
using System;
using System.Threading.Tasks;
using ApiValidation.Infra;

namespace ApiValidation.Services
{
    public class UserService : IUserService
    {
        public UserService() { }

        public async Task<User> CreateUser(string name, string email, string password)
        {
            var encryptedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                EncryptedPassword = encryptedPassword
            };

            var database = new Database();
            await database.AddUser(user);

            return user;
        }
    }
}
