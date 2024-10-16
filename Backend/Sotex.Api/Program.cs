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


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<OpenAISettings>(builder.Configuration.GetSection("OpenAI"));
builder.Services.AddHttpClient<IMenuService, MenuService>();
builder.Services.AddScoped<ResizeImage>();

//builder.Services.AddAuthentication().AddGoogle(googleOptions =>
//{
//    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
//    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
//});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddDbContext<ProjectDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ProjectDatabase")));

builder.Services.AddScoped<UserRepo>();
builder.Services.AddScoped<MenuRepo>();
builder.Services.AddScoped<OrdersRepo>();
builder.Services.AddScoped<OrderedMenuItemsRepo>();

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();