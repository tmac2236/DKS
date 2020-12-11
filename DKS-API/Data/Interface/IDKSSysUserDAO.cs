
using System.Threading.Tasks;
using DFPS.API.Models.DKSSys;
using DFPS_API.Data.Interface;

namespace DFPS_API.Data.Repository.Interfaces
{
    public interface IDKSSysUserDAO : ICommonDAO<SysUser>
    {
        Task<SysUser> Login(string username);
    }
}