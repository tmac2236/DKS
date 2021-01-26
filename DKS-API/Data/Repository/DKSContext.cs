using Microsoft.EntityFrameworkCore;
using DKS.API.Models.DKS;
using DKS_API.DTOs;

namespace DKS_API.Data.Repository
{
    public class DKSContext : DbContext
    {
        //Constructor
        public DKSContext(DbContextOptions<DKSContext> options) : base(options) { }
        //EF(SHCDEV3)
        public DbSet<Ordsumoh> ORDSUMOH { get; set; }
        public DbSet<UserLog> USER_LOG { get; set; }

        public DbSet<Staccrth> STACCRTH { get; set; }
        public DbSet<Pracdath> PRACDATH { get; set; }
        public DbSet<Pracdatb> PRACDATB { get; set; }
        public DbSet<Proporh> PROPORH { get; set; }


        //DTO(Stored Procedure)
        public DbSet<F418_F420Dto> GetF420F418View { get; set; }
        public DbSet<F340_ProcessDto> GetF340ProcessView{get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ordsumoh>().HasKey(x => new { x.PRSUMNO });
            modelBuilder.Entity<UserLog>().HasKey(x => new { x.LOGINNAME, x.UPDATETIME });
            modelBuilder.Entity<Staccrth>().HasKey(x => new { x.PKPLGHID });
            modelBuilder.Entity<Pracdath>().HasKey(x => new { x.PROACCNO });
            modelBuilder.Entity<Pracdatb>().HasKey(x => new { x.PKPRACBID, x.PROACCNO });
            modelBuilder.Entity<Proporh>().HasKey(x => new { x.PROORDNO });


            //DTO(Stored Procedure)
            modelBuilder.Entity<F418_F420Dto>()
            .HasNoKey();
            modelBuilder.Entity<F340_ProcessDto>()
            .HasNoKey();

        }
    }
}