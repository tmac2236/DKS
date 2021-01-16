using System;
using System.Threading.Tasks;
using DKS.API.Models.DKSSys;
using DKS_API.Data.Interface;
using Microsoft.EntityFrameworkCore;

namespace DKS_API.Data.Repository
{
    public class UserDAO : CommonDAO<User>, IUserDAO
    {
        public UserDAO(DataContext context): base(context)
        {
        }
        public async Task<User> Login(string account, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.Account == account);
            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.CreatedTime = DateTime.Now;

            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string account)
        {
            if (await _context.User.AnyAsync(x => x.Account == account))
                return true;

            return false;

        }

    }
}