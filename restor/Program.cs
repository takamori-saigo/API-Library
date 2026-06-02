using AutoMapper;
using Contracts;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Repository;
using restor;

var builder = WebApplication.CreateBuilder(args);
var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
builder.Services.AddControllers(x => { x.RespectBrowserAcceptHeader = true;
    x.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson()
    .AddXmlSerializerFormatters();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});
builder.Services.AddDbContext<RestorDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString(nameof(RestorDbContext))));
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

logger.Info("start aplication");
var app = builder.Build();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "Hello World!");
app.Run();