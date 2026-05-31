using Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using NLog;
using Repository;

var builder = WebApplication.CreateBuilder(args);
var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
builder.Services.AddControllers(x => { x.RespectBrowserAcceptHeader = true;
    x.ReturnHttpNotAcceptable = true;
}).AddXmlSerializerFormatters();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
builder.Services.AddDbContext<RestorDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString(nameof(RestorDbContext))));
logger.Info("start aplication");
var app = builder.Build();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "Hello World!");
app.Run();