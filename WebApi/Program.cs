using DapperSamples.Authorization;
using DapperSamples.Authorization.Jwt;
using DapperSamples.Database;
using Microsoft.OpenApi.Models;
using ProjectManagmentAPI.Authorization.Providers;
using ProjectManagmentAPI.Database.Repository;
using ProjectManagmentAPI.Features.Priorities.Repositories;
using ProjectManagmentAPI.Features.Projects.Repositories;
using ProjectManagmentAPI.Features.Statuses.Repositories;
using ProjectManagmentAPI.Features.Users.Repositories;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddScoped<ITokenDataProvider, TokenDataProvider>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStatusesRepository, StatusesRepository>();
builder.Services.AddScoped<IPrioritiesRepository, PrioritiesRepository>();
builder.Services.AddScoped<IProjectsRepository, ProjectsRepository>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();

builder.Services.UseJwtToken(configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ProjectManagmentApi",
        Version = "v1",
        Description = "Project managment API",
    });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Bearer Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    }
                },
                new string[] {}
            }
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
