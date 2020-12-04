using Microsoft.EntityFrameworkCore;
using DFPS.API.Models;
using DFPS.API.DTOs;
using DFPS_API.DTOs;

namespace DFPS.API.Data
{
    public class DataContext : DbContext
    {
        //Constructor
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        //EF
        public DbSet<User> User { get; set; }

        public DbSet<Jang1Base> Jang1Base { get; set; }
        public DbSet<Jang1HR> Jang1HR { get; set; }
        public DbSet<SampleWorkBase> SampleWorkBase { get; set; }
        public DbSet<SampleWorkProcess> SampleWorkProcess { get; set; }
        public DbSet<SampleWorkWorker> SampleWorkWorker { get; set; }

        //DTO(Stored Procedure)
        public DbSet<GetReportDataPassDto> GetReportDataPassDto { get; set; }
        public DbSet<PDModelDto> GetPDModelDto { get; set; }
        public DbSet<AttendanceDto> GetAttendanceDto { get; set; }
        public DbSet<ChangeWorkerDto> GetChangeWorkerDto { get; set; }
        public DbSet<NoOperationDto> GetNoOperationDto { get; set; }
        public DbSet<SelectLean> GetAllLeanId { get; set; }
        public DbSet<SelectModelByLean> GetAllModelByLean { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GetReportDataPassDto>()
                .HasNoKey();
            modelBuilder.Entity<PDModelDto>()
                .HasNoKey();
            modelBuilder.Entity<AttendanceDto>()
                .HasNoKey();
            modelBuilder.Entity<ChangeWorkerDto>()
               .HasNoKey();
            modelBuilder.Entity<NoOperationDto>()
               .HasNoKey();
            modelBuilder.Entity<SelectLean>()
               .HasNoKey();
            modelBuilder.Entity<SelectModelByLean>()
               .HasNoKey();
        }
    }
}