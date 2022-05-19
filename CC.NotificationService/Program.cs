using CC.NotificationService.Interfaces;
using CC.NotificationService.Models;
using CC.NotificationService.Workers;
using FluentValidation;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add Options
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection(SmtpSettings.SectionName));

//Add Fluent validation DJ
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

//Add Scoped Services
builder.Services.AddScoped<IEmailWorker, EmailWorker>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
