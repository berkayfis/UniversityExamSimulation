using Microsoft.EntityFrameworkCore;
using UniversityExamSimulation.Core.Repositories;
using UniversityExamSimulation.Core.Services;
using UniversityExamSimulation.Data;
using UniversityExamSimulation.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<UniversityExamDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UniversityExamSimulation")));

builder.Services.AddHttpClient();

builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<IStudentRepository, StudentRepository>();
builder.Services.AddTransient<IUniversityRepository, UniversityRepository>();
builder.Services.AddTransient<IStartExamService, StartExamService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
