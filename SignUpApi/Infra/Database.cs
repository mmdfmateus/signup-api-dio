using ApiValidation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiValidation.Infra
{
    public class Database
    {
        private List<User> Users { get; set; }

        public Database() { }

        public async Task<User> AddUser(User user)
        {
            await Task.Run(() => Users.Add(user));
            return user;
        }
    }
}
