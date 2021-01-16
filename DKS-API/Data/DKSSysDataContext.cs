
using DFPS.API.Models.DKSSys;
using Microsoft.EntityFrameworkCore;

namespace DKS_API.Data.Repository
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