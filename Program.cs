using Quartz;
using Quartz.Impl;
using Quartz.Spi;

using challenge_20220626.JobFactory;
using challenge_20220626.Jobs;
using challenge_20220626.Models;
using challenge_20220626.Schedular;
using challenge_20220626.Services;

namespace challenge_20220626{

    public class Program{
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>{
                    webBuilder
                    .ConfigureServices((hostContext, services) =>{
                        // Add services to the container.
                        services.Configure<ProductDatabaseSettings>
                            (WebApplication.CreateBuilder(args).Configuration.GetSection("OpenFoodFactsDatabase"));

                        services.AddSingleton<IJobFactory, MyJobFactory>();
                        services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();                    

                        services.AddSingleton<ProductServices>();
                        services.AddSingleton<ScrapingService>();

                        services.AddControllers();
                        services.AddEndpointsApiExplorer();
                        //Descrever a documentação da API utilizando o conceito de Open API 3.0;
                        services.AddSwaggerGen();

                        #region Adding JobType
                        services.AddSingleton<NotificationJob>();
                        services.AddSingleton<LoggerJob>();
                        services.AddSingleton<CronJob>();
                        #endregion

                        #region Adding Jobs 
                        List<JobMetadata> jobMetadatas = new List<JobMetadata>();
                        // jobMetadatas.Add(new JobMetadata(Guid.NewGuid(), typeof(NotificationJob), "Notify Job", "0/10 * * * * ?"));
                        // jobMetadatas.Add(new JobMetadata(Guid.NewGuid(), typeof(LoggerJob), "Log Job", "0/20 * * * * ?"));
                        jobMetadatas.Add(new JobMetadata(Guid.NewGuid(), typeof(CronJob), "Cron Job", "0 0 3 * * ?"));
                        
                        services.AddSingleton(jobMetadatas);
                        #endregion

                        services.AddHostedService<MySchedular>();
                })
                .Configure(app =>{
                    app.UseHttpsRedirection();

                    app.UseRouting();

                    app.UseAuthorization();

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });

                    app.UseSwagger();
                    app.UseSwaggerUI(options =>{
                        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                        options.RoutePrefix = string.Empty;
                    });
                });
            });

        public static void Main(string[] args){
            CreateHostBuilder(args).Build().Run();
        }
    }
}