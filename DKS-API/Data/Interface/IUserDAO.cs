using System.Threading.Tasks;
using DFPS.API.Models;
using System.Collections.Generic;
using DFPS_API.Data.Interface;

namespace DFPS.API.Data.Interface
{
    public interface IUserDAO : ICommonDAO<User>
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);

    }
}