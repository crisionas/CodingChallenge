using CC.NotificationService.Interfaces;
using CC.NotificationService.Models;
using CC.NotificationService.Workers;
using FluentValidation;
using System.Reflection;
using CC.Common;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(7081);
});

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .ReadFrom.Configuration(ctx.Configuration));
// Add Options
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection(SmtpSettings.SectionName));

//Add Scoped Services
builder.Services.AddScoped<IEmailWorker, EmailWorker>();

builder.Services.AddFluentValidationBaseResponse(x =>
    x.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options=>options.EnableAnnotations());
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseHealthChecks("/healthy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
