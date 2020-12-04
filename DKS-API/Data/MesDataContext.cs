
using DFPS.API.Models.MES;
using Microsoft.EntityFrameworkCore;

namespace DFPS.API.Data
{
    public class MesDataContext : DbContext
    {
        public MesDataContext(DbContextOptions<MesDataContext> options) : base(options) { }
        public DbSet<MesUser> MES_User { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MesUser>().HasKey(x => new { x.Factory_ID, x.User_ID });
        }
    }
}