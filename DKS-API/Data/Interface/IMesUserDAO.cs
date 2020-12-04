
using System.Threading.Tasks;
using DFPS.API.Models.MES;
using DFPS_API.Data.Interface;

namespace DFPS_API.Data.Repository.Interfaces
{
    public interface IMesUserDAO : ICommonDAO<MesUser>
    {
        Task<MesUser> Login(string username, string password);
    }
}