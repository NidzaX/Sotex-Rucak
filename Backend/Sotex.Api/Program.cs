using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sotex.Api.Interfaces;
using OpenAI.Chat;
using Sotex.Api.Model;
using Microsoft.OpenApi.Models;
using Sotex.Api.Services;
using Sotex.Api.Services.DependencyInjection;
using Sotex.Api.Infrastructure;
using Microsoft.AspNetCore.Authentication.Google;
using Sotex.Api.Repo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Sotex.Api.Mapping;
using Microsoft.Extensions.Options;
using Sotex.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure settings and dependencies
builder.Services.Configure<OpenAISettings>(builder.Configuration.GetSection("OpenAI"));
builder.Services.Configure<GoogleSettings>(builder.Configuration.GetSection("Authentication:Google"));
builder.Services.AddHttpClient<IMenuService, MenuService>();
builder.Services.AddScoped<ResizeImage>();

// Configure Google OAuth authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "http://localhost:5105",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
    };
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});


builder.Services.AddControllers();

// Configure Swagger with JWT authentication
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var corsPolicy = "_cors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicy, builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

// Configure AutoMapper
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// Configure Database Context
builder.Services.AddDbContext<ProjectDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ProjectDatabase")));

// Configure Repositories and Services
builder.Services.AddScoped<UserRepo>();
builder.Services.AddScoped<MenuRepo>();
builder.Services.AddScoped<OrdersRepo>();
builder.Services.AddScoped<OrderedMenuItemsRepo>();

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddLogging();
builder.Services.AddScoped<ILogger<OrdersRepo>, Logger<OrdersRepo>>();
builder.Services.AddScoped<ILogger<OrderService>, Logger<OrderService>>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.ApplyMigrations();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend v1"));
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(corsPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
