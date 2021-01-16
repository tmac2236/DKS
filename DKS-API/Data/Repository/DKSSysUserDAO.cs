using System.Threading.Tasks;
using DFPS.API.Models.DKSSys;
using DKS_API.Data.Interface;
using Microsoft.EntityFrameworkCore;

namespace DKS_API.Data.Repository
{
    public class DKSSysUserDAO : DKSSysCommonDAO<SysUser>, IDKSSysUserDAO
    {
        public DKSSysUserDAO(DKSSysDataContext context) : base(context)
        {
        }
        public async Task<SysUser> Login(string account)
        {
            var user = await _context.SYS_USER.FirstOrDefaultAsync(x => x.LOGIN == account);
            if (user == null)
                return null;
            return user;
        }
    }
}