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
        public DbSet<ModelDah> MODELDAH { get; set; }
        public DbSet<Articled> ARTICLED { get; set; }
        public DbSet<DevBuyPlan> DEV_BUYPLAN { get; set; }
        public DbSet<SamPartB> SAMPARTB { get; set; }
        public DbSet<DevTreatment> DEV_TREATMENT { get; set; }

        //DTO(Stored Procedure)
        public DbSet<F418_F420Dto> GetF420F418View { get; set; }
        public DbSet<F340_ProcessDto> GetF340ProcessView { get; set; }
        public DbSet<F428SampleNoDetail> GetMaterialNoBySampleNoForWarehouseView { get; set; }
        public DbSet<StockDetailByMaterialNo> GetStockDetailByMaterialNoView { get; set; }
        public DbSet<F340_PpdDto> GetF340PpdView { get; set; }
        public DbSet<UserRoleDto> UserRoleDto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ordsumoh>().HasKey(x => new { x.PRSUMNO });
            modelBuilder.Entity<UserLog>().HasKey(x => new { x.LOGINNAME, x.UPDATETIME });
            modelBuilder.Entity<Staccrth>().HasKey(x => new { x.PKPLGHID });
            modelBuilder.Entity<Pracdath>().HasKey(x => new { x.PROACCNO });
            modelBuilder.Entity<Pracdatb>().HasKey(x => new { x.PKPRACBID, x.PROACCNO });
            modelBuilder.Entity<Proporh>().HasKey(x => new { x.PROORDNO });
            modelBuilder.Entity<ModelDah>().HasKey(x => new { x.MODELNO });
            modelBuilder.Entity<Articled>().HasKey(x => new { x.PKARTBID });
            modelBuilder.Entity<DevBuyPlan>().HasKey(x => new { x.MANUF, x.SEASON, x.MODELNO, x.SCOLOR, x.ARTICLE, x.VERN });
            modelBuilder.Entity<SamPartB>().HasKey(x => new { x.PARTNO, x.SAMPLENO });
            modelBuilder.Entity<DevTreatment>().HasKey(x => new { x.PARTNO,x.SAMPLENO,x.TREATMENTCODE,x.VERNO });

            //DTO(Stored Procedure)
            modelBuilder.Entity<F418_F420Dto>()
            .HasNoKey();
            modelBuilder.Entity<F340_ProcessDto>()
            .HasNoKey();
            modelBuilder.Entity<F428SampleNoDetail>()
            .HasNoKey();
            modelBuilder.Entity<StockDetailByMaterialNo>()
            .HasNoKey();
            modelBuilder.Entity<F340_PpdDto>()
            .HasNoKey();
            modelBuilder.Entity<UserRoleDto>()
            .HasNoKey();

        }
    }
}