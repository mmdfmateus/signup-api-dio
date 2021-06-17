using ApiValidation.Models;
using System.Threading.Tasks;

namespace ApiValidation.Services
{
    public interface IUserService
    {
        Task<User> CreateUser(string name, string email, string password);
    }
}
