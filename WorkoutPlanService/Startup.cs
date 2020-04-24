using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Security.Cryptography;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WorkoutPlanService.DataAccessPoint.Configuration;
using WorkoutPlanService.Domain.Configuration;

namespace WorkoutPlanService
{
    public sealed class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddWorkoutPlanServiceDomain();
            services.AddWorkoutPlanServiceDataAccessPointOptions(Configuration);
            services.AddWorkoutPlanServiceDataAccessPoint(Configuration.GetConnectionString("WorkoutPlanDatabase"));
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
            ConfigureAuthorization(services);
        }

        private void ConfigureAuthorization(IServiceCollection services)
        {
            var rsa = RSA.Create(new RSAParameters
            {
                Exponent = Convert.FromBase64String(Configuration.GetValue<string>("Authorization:Exponent")),
                Modulus = Convert.FromBase64String(Configuration.GetValue<string>("Authorization:Modulus"))
            });
            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new RsaSecurityKey(rsa),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        NameClaimType = "Name"
                    };
                });
        }

        public void Configure(IApplicationBuilder app, IBackgroundJobClient backgroundClient, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //RAM Heavy //CPU Low optimalization
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseWorkoutPlanServiceDataAccessPoint(backgroundClient);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
