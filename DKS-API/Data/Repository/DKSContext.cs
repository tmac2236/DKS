using Microsoft.EntityFrameworkCore;
using DFPS.API.Models;
using DFPS.API.DTOs;
using DFPS_API.DTOs;
using DFPS.API.Models.DKSSys;

namespace DFPS.API.Data
{
    public class DKSContext : DbContext
    {
        //Constructor
        public DKSContext(DbContextOptions<DKSContext> options) : base(options) { }
        //EF
        public DbSet<Ordsumoh> ORDSUMOH { get; set; }

        //DTO(Stored Procedure)

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ordsumoh>().HasKey(x => new { x.PRSUMNO });
        }
    }
}