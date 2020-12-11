
using DFPS.API.Models.DKSSys;
using Microsoft.EntityFrameworkCore;

namespace DFPS.API.Data
{
    public class DKSSysDataContext : DbContext
    {
        public DKSSysDataContext(DbContextOptions<DKSSysDataContext> options) : base(options) { }
        public DbSet<SysUser> SYS_USER { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SysUser>().HasKey(x => new { x.USERID });
        }
    }
}