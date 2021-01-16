
using System.Threading.Tasks;
using DFPS.API.Models.DKSSys;

namespace DKS_API.Data.Interface
{
    public interface IDKSSysUserDAO : ICommonDAO<SysUser>
    {
        Task<SysUser> Login(string username);
    }
}