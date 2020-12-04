using System.Threading.Tasks;
using DFPS.API.Data;
using DFPS.API.Models.MES;
using DFPS_API.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DFPS_API.Data.Repository
{
    public class MesUserDAO : MesCommonDAO<MesUser>, IMesUserDAO
    {
        public MesUserDAO(MesDataContext context) : base(context)
        {
        }
        public async Task<MesUser> Login(string account, string password)
        {
            var user = await _context.MES_User.FirstOrDefaultAsync(x => x.User_ID == account && x.Password == password);
            if (user == null)
                return null;
            return user;
        }
    }
}