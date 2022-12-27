using System.Net;
using System.Reflection;
using AsrTool;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Common;
using AsrTool.Infrastructure.Common.Imp;
using AsrTool.Infrastructure.Common.Policy;
using AsrTool.Infrastructure.Common.Translation;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Context.Seeders;
using AsrTool.Infrastructure.Context.Seeders.Imp;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Domain.Objects.Configurations;
using AsrTool.Infrastructure.Extensions;
using AsrTool.Infrastructure.Jobs;
using AsrTool.Infrastructure.Jobs.Imp;
using AsrTool.Infrastructure.MediatR.Behaviors;
using AsrTool.Middlewares;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var DevAllowSpecificOrigins = "DevAllowSpecificOrigins";

// SETUP LOGGER
var logConfig = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        .Build();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(logConfig)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// SETTINGS
var appSettingsSection = builder.Configuration.GetSection(nameof(AppSettings));
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();

// SERVICES
builder.Services.AddAuthorizers();
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(Program))!);

builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
  options.AddPolicy(name: DevAllowSpecificOrigins,
    builder =>
    {
      builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(Program)));

builder.Services.AddScoped<ICacheService>(p =>
  new CacheService(
    p.GetRequiredService<ILogger<CacheService>>(),
    new AsrContext(p.GetRequiredService<DbContextOptions>()),
    p.GetRequiredService<IMemoryCache>()));
builder.Services.AddScoped<IUserResolver>(p =>
  new UserResolver(new User(p.GetService<IHttpContextAccessor>()?.HttpContext?.User, p.GetRequiredService<ICacheService>())));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSingleton<ITranslateService, TranslateService>();

// Seeders
builder.Services.AddScoped<ISeeder, EmployeeSeeder>();
builder.Services.AddScoped<ISeeder, RoleSeeder>();
builder.Services.AddSingleton<IStore, Store>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie(opt => ApplyCookieOption(opt));

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
builder.Services.AddSingleton<ILdapConnector, LdapConnector>();
builder.Services.AddScoped<IUserManager, UserManager>();


// DB context
builder.Services.AddDbContext<AsrContext>(opt => opt.UseSqlServer(appSettings.AsrToolDbConnectionString));
builder.Services.Remove(builder.Services.Single(x => x.ServiceType == typeof(AsrContext)));
builder.Services.AddScoped<IAsrContext, AsrContext>(p => new AsrContext(p.GetRequiredService<DbContextOptions>(), p.GetRequiredService<IUserResolver>()));

// Memory Cache
builder.Services.AddMemoryCache();

// Log
builder.Services.AddLogging();

// APP
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(DevAllowSpecificOrigins);

app.UseAuthentication();
app.UseMiddleware<CookieOnlyAuthenticationMiddleware>();

app.UseRouting();

app.UseAuthorization();

InitDatabase(app).RunAwait();
MigrateContext(app).RunAwait();

if (!IsSeeded(app).RunAwait())
{
  SeedData(app).RunAwait();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();


static async Task InitDatabase(IApplicationBuilder appBuilder)
{
  using var scope = appBuilder.ApplicationServices.CreateScope();
  var context = scope.ServiceProvider.GetRequiredService<IAsrContext>();
  if (!context.IsDatabaseExist)
  {
    await context.MigrateAsync("InitDatabase");
  }
}

static async Task MigrateContext(IApplicationBuilder appBuilder)
{
  using var scope = appBuilder.ApplicationServices.CreateScope();
  var context = scope.ServiceProvider.GetRequiredService<IAsrContext>();
  await context.MigrateAsync();
}

static async Task SeedData(IApplicationBuilder appBuilder)
{
  using var scope = appBuilder.ApplicationServices.CreateScope();
  var userResolver = scope.ServiceProvider.GetRequiredService<IUserResolver>();
  var appSetting = scope.ServiceProvider.GetService<IOptions<AppSettings>>()?.Value;

  using var _ = userResolver.UseSystemUser();
  var seederServices = scope.ServiceProvider.GetServices<ISeeder>();
  foreach (var seederService in seederServices.OrderBy(x => x.Priority))
  {
    await seederService.Seed();
  }
  await scope.ServiceProvider.GetRequiredService<IAsrContext>().SaveChangesAsync();
}

static async Task<bool> IsSeeded(IApplicationBuilder applicationBuilder)
{
  using var scope = applicationBuilder.ApplicationServices.CreateScope();
  var context = scope.ServiceProvider.GetRequiredService<IAsrContext>();
  return await context.Get<Role>().AnyAsync();
}

static CookieAuthenticationOptions ApplyCookieOption(CookieAuthenticationOptions options)
{
  options.Cookie.Name = "AsrTool.Auth";
  options.SlidingExpiration = true;
  options.ExpireTimeSpan = TimeSpan.FromMinutes(360);
  options.Events = new CookieAuthenticationEvents
  {
    OnRedirectToLogin = context =>
    {
      context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
      return Task.CompletedTask;
    },
    OnRedirectToAccessDenied = context =>
    {
      context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
      return Task.CompletedTask;
    },

    OnValidatePrincipal = (context) => Task.CompletedTask
  };

  return options;
}
