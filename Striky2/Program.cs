﻿using Microsoft.AspNetCore.Identity;
using Striky.Api.Repository;
using Striky.Api.Services;
using Striky2.Models;
using Striky2.Services.ExerciesDetailsServcies;
using Striky2.Services.ExerciesServcies;
using Striky2.Services.Users;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Db16821Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("con")));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<Db16821Context>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICategoryServices,CategoryServices>();
 builder.Services.AddScoped<CategoryServices>();
builder.Services.AddScoped<IExerciesServices,ExerciesServices>();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("UploadPaths"));
builder.Services.AddTransient<IExerciesDetailsServcies, ExerciesDetailsServcies>();
// أضف هذا قبل تسجيل UserServices
var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "Images");
builder.Services.AddSingleton(imagePath);

// ثم سجل الخدمة
builder.Services.AddScoped<IUserServices, UserServices>(); builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
