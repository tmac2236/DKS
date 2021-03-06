using System.Threading.Tasks;
using DKS.API.Models.DKSSys;

namespace DKS_API.Data.Interface
{
    public interface IUserDAO : ICommonDAO<User>
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);

    }
}