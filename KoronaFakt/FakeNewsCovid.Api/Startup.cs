using FakeNewsCovid.Domain.Context;
using FakeNewsCovid.Domain.Helper;
using FakeNewsCovid.Domain.Services;
using FakeNewsCovid.Domain.Services.Base;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace FakeNewsCovid.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMediatR(typeof(FakeNewsCovidContext));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FakeNewsCovid API", Version = "v1" });
            });
            services.AddDbContext<FakeNewsCovidContext>(options =>
                options.UseNpgsql(@"Server=77.55.226.197;Port=5432;Database=korona;User Id=dev;Password=korona@3341_fakt#45da@@34;"));
            services.AddScoped<IFakeNewsDbService, FakeNewsDbService>();
            services.AddScoped<IElasticSearchService, ElasticSearchService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
