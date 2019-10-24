using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Hangfire.Mongo;
using Hangfire.Domain;

namespace Hangfire.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var migrationOptions = new MongoMigrationOptions
            {
                Strategy = MongoMigrationStrategy.Migrate,
                BackupStrategy = MongoBackupStrategy.Collections
            };
            var storageOptions = new MongoStorageOptions
            {
                MigrationOptions = migrationOptions
            };

            services.AddHangfire(x => 
                x.UseMongoStorage("mongodb://localhost:17017", "Hangfire", storageOptions));

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Exemplo Hangfire - github.com/marraia");
            });

            InitProcess();
        }

        private void InitProcess()
        {
            var division = new Division();
            RecurringJob.AddOrUpdate(() => division.DivisionRandom(), Cron.Minutely());
        }
    }
}
