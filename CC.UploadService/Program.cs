using CC.Common;
using CC.UploadService.Infrastructure;
using Hangfire;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(7080);
});

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .ReadFrom.Configuration(ctx.Configuration));

builder.Services.BindServices(builder.Configuration);

builder.Services.AddFluentValidationBaseResponse(x => 
    x.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenOptions();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseHealthChecks("/healthy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
