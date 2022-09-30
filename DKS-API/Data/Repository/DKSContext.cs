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
        public DbSet<ModelDab> MODELDAB { get; set; }
        public DbSet<Articled> ARTICLED { get; set; }
        public DbSet<DevBuyPlan> DEV_BUYPLAN { get; set; }
        public DbSet<SamPartB> SAMPARTB { get; set; }
        public DbSet<DevTreatment> DEV_TREATMENT { get; set; }
        public DbSet<DevTreatmentFile> DEV_TREATMENT_FILE { get; set; }
        public DbSet<DevSysSet> DEV_SYSSET { get; set; }
        public DbSet<ArticledLdtm> ARTICLED_LDTM { get; set; }
        public DbSet<ArticlePicture> ARTICLE_PICTURE { get; set; }
        public DbSet<DevDtrFgt> DTR_FGT { get; set; }
        public DbSet<DevDtrFgtResult> DTR_FGT_RESULT { get; set; }
        public DbSet<DevDtrVsFile> DTR_VS_FILE { get; set; }
        public DbSet<DtrLoginHistory> DTR_LOGIN_HISTORY { get; set; }
        public DbSet<EmpDataH> EMPDATAH { get; set; }
        public DbSet<DevPlmPart> DEV_PLM_PART { get; set; }
        public DbSet<DevSendMail> DEV_SENDMAIL { get; set; }
        public DbSet<DtrFgtShoes> DTR_FGT_SHOES { get; set; }
        public DbSet<DtrFgtEtd> DTR_FGT_ETD { get; set; }

        public DbSet<SamDetlB> SAMDETLB { get; set; }
        public DbSet<TempSamplQtb> TEMPSAMPLQTB { get; set; }
        


        //DTO(Stored Procedure)
        public DbSet<F418_F420Dto> GetF420F418View { get; set; }
        public DbSet<F340_ProcessDto> GetF340ProcessView { get; set; }
        public DbSet<F428SampleNoDetail> GetMaterialNoBySampleNoForWarehouseView { get; set; }
        public DbSet<StockDetailByMaterialNo> GetStockDetailByMaterialNoView { get; set; }
        public DbSet<F340_PpdDto> GetF340PpdView { get; set; }
        public DbSet<UserRoleDto> UserRoleDto { get; set; }
        public DbSet<P206DataByStageArticleDto> GetP206DataByArticle { get; set; }
        public DbSet<DevDtrFgtResultDto> GetDevDtrFgtResultDto { get; set; }
        public DbSet<F340PartNoTreatmemtDto> GetF340PartNoTreatmemtDto { get; set; }
        public DbSet<ArticleModelNameDto> GetArticleModelNameDto { get; set; }
        public DbSet<ArticleSeasonDto> GetArticleSeasonDto { get; set; }
        public DbSet<DevDtrVsListDto> GetDevDtrVsListDto { get; set; }
        public DbSet<BasicCodeDto> GetBasicCodeDto { get; set; }
        public DbSet<TupleDto> GetTupleDto { get; set; }
        public DbSet<SampleTrackReportDto> GetSampleTrackReportDto { get; set; }

        public DbSet<DtrFgtEtdDto> GetDtrFgtEtdDto { get; set; }
        public DbSet<NoneDto> GetNoneDto { get; set; }
        public DbSet<P202Dto> GetP202Dto { get; set; }
        public DbSet<KanbanTQCDto> GetKanbanTQCDto { get; set; }
        public DbSet<KanbanDataByLineDto> GetKanbanDataByLineDto { get; set; }
        public DbSet<F406iDto> GetF406iDto{ get; set; }
        public DbSet<P406Dto> GetP406Dto{ get; set; }
        public DbSet<F434Dto> GetF434Dto{ get; set; }
        public DbSet<F505Dto> GetF505Dto{ get; set; }
        public DbSet<CheckF303Dto> GetCheckF303Dto{ get; set; }
        public DbSet<GetF303MatQtyDto> GetF303MatQtyDto{ get; set; }    
        public DbSet<GetF303PartQtyDto> GetF303PartQtyDto{ get; set; }  
        public DbSet<DtrF206BomDto> GetDtrF206BomDto{ get; set; }
        public DbSet<SrfChangeDto> GetSrfChangeDto { get; set; }      
        public DbSet<SrfDifferenceDto> GetSrfDifferenceDto { get; set; }            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ordsumoh>().HasKey(x => new { x.PRSUMNO });
            modelBuilder.Entity<UserLog>().HasKey(x => new { x.LOGINNAME, x.UPDATETIME });
            modelBuilder.Entity<Staccrth>().HasKey(x => new { x.PKPLGHID });
            modelBuilder.Entity<Pracdath>().HasKey(x => new { x.PROACCNO });
            modelBuilder.Entity<Pracdatb>().HasKey(x => new { x.PKPRACBID, x.PROACCNO });
            modelBuilder.Entity<Proporh>().HasKey(x => new { x.PROORDNO });
            modelBuilder.Entity<ModelDah>().HasKey(x => new { x.MODELNO,x.FACTORYID });
            modelBuilder.Entity<ModelDab>().HasKey(x => new { x.MODELNO,x.SHOESIZE });
            modelBuilder.Entity<Articled>().HasKey(x => new { x.PKARTBID });
            modelBuilder.Entity<DevBuyPlan>().HasKey(x => new { x.MANUF, x.SEASON, x.MODELNO, x.SCOLOR, x.ARTICLE, x.VERN });
            modelBuilder.Entity<SamPartB>().HasKey(x => new { x.PARTNO, x.SAMPLENO });
            modelBuilder.Entity<DevTreatment>().HasKey(x => new { x.PARTNO, x.SAMPLENO, x.TREATMENTCODE, x.VERNO, x.FACTORYID });
            modelBuilder.Entity<DevTreatmentFile>().HasKey(x => new { x.ARTICLE, x.PARTNO, x.TREATMENTCODE, x.UPTIME });
            modelBuilder.Entity<DevSysSet>().HasKey(x => new { x.SYSKEY });
            modelBuilder.Entity<ArticledLdtm>().HasKey(x => new { x.PKARTBID });
            modelBuilder.Entity<ArticlePicture>().HasKey(x => new { x.FKARTICID });
            modelBuilder.Entity<DtrLoginHistory>().HasKey(x => new { x.ID });
            modelBuilder.Entity<EmpDataH>().HasKey(x => new { x.WORKPNO });
            modelBuilder.Entity<DevDtrFgt>().HasKey(x => new { x.ARTICLE, x.STAGE, x.KIND, x.VERN });
            modelBuilder.Entity<DevDtrFgtResult>().HasKey(x => new { x.ARTICLE, x.MODELNO, x.MODELNAME, x.LABNO, x.STAGE, x.KIND });
            modelBuilder.Entity<DevDtrVsFile>().HasKey(x => new { x.FACTORYID,x.ARTICLE, x.SEASON, x.ID });
            modelBuilder.Entity<DevPlmPart>().HasKey(x => new { x.PARTNO });
            modelBuilder.Entity<DevSendMail>().HasKey(x => new { x.WORKPNO,x.DEPTID,x.EMAIL_TYPE,x.DEVTEAM });
            modelBuilder.Entity<DtrFgtShoes>().HasKey(x => new { x.SAMPLENO });
            modelBuilder.Entity<DtrFgtEtd>().HasKey(x => new { x.FACTORYID,x.ARTICLE,x.STAGE,x.TEST,x.QC_RECEIVE });
            modelBuilder.Entity<SamDetlB>().HasKey(x => new { x.PKSAMDLID,x.SAMPLENO });
            modelBuilder.Entity<TempSamplQtb>().HasKey(x => new { x.SAMPLENO,x.PASSIDNAME });
            

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
            modelBuilder.Entity<P206DataByStageArticleDto>()
            .HasNoKey();
            modelBuilder.Entity<DevDtrFgtResultDto>()
            .HasNoKey();
            modelBuilder.Entity<F340PartNoTreatmemtDto>()
            .HasNoKey();
            modelBuilder.Entity<ArticleModelNameDto>()
            .HasNoKey();
            modelBuilder.Entity<ArticleSeasonDto>()
            .HasNoKey();
            modelBuilder.Entity<DevDtrVsListDto>()
            .HasNoKey();
            modelBuilder.Entity<BasicCodeDto>()
            .HasNoKey();
            modelBuilder.Entity<TupleDto>()
            .HasNoKey();
            modelBuilder.Entity<SampleTrackReportDto>()
            .HasNoKey();
            modelBuilder.Entity<DtrFgtEtdDto>()
            .HasNoKey();            
            modelBuilder.Entity<NoneDto>()
            .HasNoKey();
            modelBuilder.Entity<P202Dto>()
            .HasNoKey(); 
            modelBuilder.Entity<KanbanTQCDto>()
            .HasNoKey();
            modelBuilder.Entity<KanbanDataByLineDto>()
            .HasNoKey(); 
            modelBuilder.Entity<F406iDto>()
            .HasNoKey();                           
            modelBuilder.Entity<P406Dto>()
            .HasNoKey(); 
            modelBuilder.Entity<F434Dto>()
            .HasNoKey(); 
            modelBuilder.Entity<F505Dto>()
            .HasNoKey();
            modelBuilder.Entity<CheckF303Dto>()
            .HasNoKey();
            modelBuilder.Entity<GetF303MatQtyDto>()
            .HasNoKey();
            modelBuilder.Entity<GetF303PartQtyDto>()
            .HasNoKey();
            modelBuilder.Entity<DtrF206BomDto>()
            .HasNoKey(); 
            modelBuilder.Entity<SrfChangeDto>()
            .HasNoKey();  
            modelBuilder.Entity<SrfDifferenceDto>()
            .HasNoKey();                                            
            
        }
    }
}