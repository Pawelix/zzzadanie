using System;
using System.Collections.Generic;
using API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NpgsqlTypes;
using Serilog;
using Serilog.Sinks.PostgreSQL.ColumnWriters;

namespace API
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

            services.AddControllers();
            services.AddScoped<ITextService, TextService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }
            //Log.Logger = new LoggerConfiguration().WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day).CreateLogger();
            string connectionString = "User ID=postgres;Password=753951;Host=localhost;Port=5432;Database=TestDb;";
            string tableName = "logs";

            IDictionary<string, ColumnWriterBase> columnOptions = new Dictionary<string, ColumnWriterBase>
                {
                    { "message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
                    { "level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                    { "raise_date", new TimestampColumnWriter(NpgsqlDbType.TimestampTz) },
                    { "properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
                };

            Log.Logger = new LoggerConfiguration().WriteTo.PostgreSQL(connectionString, 
                tableName, 
                columnOptions, 
                needAutoCreateTable: true, 
                needAutoCreateSchema: true, 
                schemaName: "logs", useCopy: true, 
                queueLimit: 3000, 
                batchSizeLimit: 40, 
                period: new TimeSpan(0, 0, 10), 
                formatProvider: null)
                .CreateLogger();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
