
using Microsoft.AspNetCore.Identity;
using TeamTaskManager.API.Middlewares;
using TeamTaskManager.Application;
using TeamTaskManager.Infrastructure;
using TeamTaskManager.Infrastructure.Data;
using Microsoft.OpenApi;
using TeamTaskManager.API.Filters;

namespace TeamTaskManager.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Team Task Manager API",
                    Version = "v1"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token here"
                });

                options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", doc)] = []
                });

            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", policy =>
                {
                    policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
                });
            });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseHttpsRedirection();
            app.UseCors("AllowAngular");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                await AppDbContextSeed.SeedRolesAsync(roleManager);
            }

            app.Run();
        }
    }
}
