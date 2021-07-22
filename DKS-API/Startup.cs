using System.Text;
using DFPS.API.Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

using AutoMapper;
using DKS_API.Data;
using DKS_API.Data.Repository;
using DKS_API.Data.Interface;
using DKS_API.Services.Implement;
using DKS_API.Filters;
using DKS_API.Helpers;
using Bottom_API.Helpers;
using DKS_API.Services.Interface;
using Quartz;
using Quartz.Spi;
using Quartz.Impl;
using DFPS_API.Quartz;
using DFPS_API.Quartz.Jobs;
using DKS_API.Helpers.AutoMapper;

namespace DKS_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //security
            services.AddCors();
            services.AddDbContext<DKSSysDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DKSSysConnection")));
            services.AddDbContext<DKSContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DKSConnection")));

            services.AddControllers();
            /*
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            */
            services.AddMvc(option => option.EnableEndpointRouting = false)
            .AddSessionStateTempDataProvider()
              .AddNewtonsoftJson(opt =>
              {
                  opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
              });

            //Auto Mapper
            services.AddAutoMapper (typeof (Startup));
            services.AddScoped<IMapper> (sp => {
                return new Mapper (AutoMapperConfig.RegisterMappings ());
            });
            services.AddSingleton (AutoMapperConfig.RegisterMappings ());
            //DAO

            services.AddScoped<IDKSSysUserDAO, DKSSysUserDAO>();
            services.AddScoped<IDKSDAO, DKSDAO>();
            services.AddScoped<IModelDahDAO, ModelDahDAO>();
            services.AddScoped<IArticledDAO, ArticledDAO>();
            services.AddScoped<IDevBuyPlanDAO, DevBuyPlanDAO>();
            services.AddScoped<IWarehouseDAO, WarehouseDAO>();
            services.AddScoped<ISamPartBDAO,SamPartBDAO>();
            services.AddScoped<IDevTreatmentDAO,DevTreatmentDAO>();
            services.AddScoped<IDevTreatmentFileDAO,DevTreatmentFileDAO>();
            services.AddScoped<IDevSysSetDAO,DevSysSetDAO>();
            services.AddScoped<IArticledLdtmDAO,ArticledLdtmDAO>();
            services.AddScoped<IDevDtrFgtDAO,DevDtrFgtDAO>();
            services.AddScoped<IDevDtrFgtResultDAO,DevDtrFgtResultDAO>();
            

            services.AddScoped<IMailUtility,MailUtility>();
            
            //Service
            services.AddScoped<ISendMailService, SendMailService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IExcelService, ExcelService>();
            services.AddScoped<IF340CheckService, F340CheckService>();

            //Add Quartz Service
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            //Add Jobs
            //services.AddSingleton<F340CheckTimeJob>();
            services.AddSingleton<SentStanMailTimeJob>();
            //Add Triggers 
            //services.AddSingleton(
            //     new JobSchedule(jobType: typeof(F340CheckTimeJob), cronExpression: " * 30 8-15 ? * MON,TUE,WED,THU,FRI,SAT *")//每五秒鐘觸發一次
            //);
            services.AddSingleton(
                 new JobSchedule(jobType: typeof(SentStanMailTimeJob), cronExpression: "0 30 8,9,10,11,12,13,14,15 ? * MON,TUE,WED,THU,FRI,SAT")
            );
            //Launch QuartzHostedServie
            services.AddHostedService<QuartzHostedService>();
            
            //auth
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                        .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            //AOP
            services.AddScoped<ApiExceptionFilter>();
            services.AddScoped<ApiActionFilter>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //疑似不會跑到此行，因為已有了ApiExceptionFilter
                app.UseExceptionHandler(
                    builder =>
                    {
                        builder.Run(async context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                            var error = context.Features.Get<IExceptionHandlerFeature>();
                            if (error != null)
                            {
                                context.Response.AddApplicationError(error.Error.Message);
                                await context.Response.WriteAsync(error.Error.Message);
                            }

                        });
                    }
                );
            }

            //security
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseHttpsRedirection();
            //auth
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
