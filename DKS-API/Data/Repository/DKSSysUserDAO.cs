using System.Threading.Tasks;
using DFPS.API.Data;
using DFPS.API.Models.DKSSys;
using DFPS_API.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DFPS_API.Data.Repository
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