using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using UnsDashboard.Server.Model;

namespace UnsDashboard.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMemoryCache(a =>
            {
                a.ExpirationScanFrequency = TimeSpan.FromSeconds(600);
            });

            builder.Services.AddDbContext<DatabaseContext>(options => DatabaseContext.GetOptions(
                builder.Configuration.GetConnectionString("DatabaseContext"),
                options));

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
           
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        var cache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();

                        context.Options.TokenValidationParameters.IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                        {
                            if (!cache.TryGetValue(nameof(TokenValidationParameters.IssuerSigningKey), out JsonWebKey key))
                            {
                                using (var client = new HttpClient())
                                {
                                    var response = client.GetAsync(parameters.ValidIssuer + "/.well-known/keys").ConfigureAwait(false).GetAwaiter().GetResult();
                                    response.EnsureSuccessStatusCode();
                                    
                                    string json = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                                    var keyset = new JsonWebKeySet(json);
                                    key = keyset.Keys.FirstOrDefault();

                                    cache.Set(
                                        nameof(TokenValidationParameters.IssuerSigningKey), 
                                        key,
                                        new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1)));
                                }
                            }

                            return new[] { key };
                        };

                        return Task.CompletedTask;
                    }
                };

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = false,
                    ValidIssuer = "https://opcfoundation.org"
                };
            });

            var app = builder.Build();

            app.UseDefaultFiles();

            app.UseStaticFiles();
            
            app.UseSession();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseAuthentication();
            app.MapControllers();
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
