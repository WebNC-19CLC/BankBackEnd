using AutoMapper;
using AsrTool.Infrastructure.Auth;
using AsrTool.Infrastructure.Common;
using AsrTool.Infrastructure.Common.Imp;
using AsrTool.Infrastructure.Common.Policy;
using AsrTool.Infrastructure.Common.Translation;
using AsrTool.Infrastructure.Context.Seeders;
using AsrTool.Infrastructure.Context.Seeders.Imp;
using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.Domain.Objects.Configurations;
using AsrTool.Infrastructure.Extensions;
using AsrTool.Infrastructure.MediatR.Common.Interfaces;
using AsrTool.UnitTest._Common.Extensions;
using AsrTool.UnitTest._Common.Fake;
using AsrTool.UnitTest._Common.Mocks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using AsrTool.Infrastructure.Context;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AsrTool.UnitTest
{
  public abstract class BaseTest : IDisposable
  {
    private static IMapper Mapper { get; set; }

    private FakeUnitTestUser UnitTestUser { get; set; }

    protected IUser TestUser => UnitTestUser;

    protected AppSettings Settings { get; private set; }

    protected IServiceProvider Container { get; private set; }

    static BaseTest()
    {
      Mapper = new Mapper(new MapperConfiguration(expression =>
        expression.AddMaps(Assembly.GetAssembly(typeof(Program)))));
    }

    // Setup
    protected BaseTest()
    {
      Settings = InitializeSetting();
      UnitTestUser = InitializeUser();

      var services = new ServiceCollection();

      services.AddSingleton<IOptions<AppSettings>>(_ => new FakeOptions<AppSettings>(Settings));

      // MediatR
      services.AddAuthorizers(Assembly.GetAssembly(typeof(Program))!)
        .AddHandlers(Assembly.GetAssembly(typeof(Program))!)
        .AddValidator(Assembly.GetAssembly(typeof(Program))!);

      // Mapper
      services.AddSingleton(_ => Mapper);

      // Services
      services.AddScoped(typeof(Lazy<>));
      services.AddScoped<IAccessPolicyService, AccessPolicyService>();
      services.AddScoped<ICacheService>(p =>
        new CacheService(
          p.GetRequiredService<ILogger<CacheService>>(),
          new AsrContext(p.GetRequiredService<DbContextOptions>()),
          p.GetRequiredService<IMemoryCache>()));
      services.AddScoped<IUserResolver>(provider => new UserResolver(TestUser));
      services.AddScoped<IEmailService, EmailService>();
      services.AddSingleton<ITranslateService, MockTranslateService>();

      // Seeder
      services.AddScoped<ISeeder, EmployeeSeeder>();
      services.AddScoped<ISeeder, RoleSeeder>();
      services.AddSingleton<IStore, Store>();

      // Auth
      services.AddAuthorization();
      services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
      services.AddSingleton<ILdapConnector, LdapConnector>();
      services.AddScoped<IUserManager, UserManager>();

      // Authorization on Entity
      services.AddScoped<IAuthorizationHandler, EmployeeAuthorizationHandler>();
      services.AddScoped<IAuthorizationHandler, RoleAuthorizationHandler>();

      // Context, using of memory database
      services.AddDbContext<AsrContext>(opt =>
        opt.UseInMemoryDatabase(Guid.NewGuid().ToString())
          .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
      services.Remove(services.Single(x => x.ServiceType == typeof(AsrContext)));
      services.AddScoped<IAsrContext, AsrContext>(p =>
        new AsrContext(p.GetRequiredService<DbContextOptions>(), p.GetRequiredService<IUserResolver>()));

      // Memory Cache
      services.AddMemoryCache();

      // Logger
      services.AddLogging();

      // Custom service or mock
      CustomRegister(services);

      Container = services.BuildServiceProvider();
    }

    protected virtual IServiceCollection CustomRegister(IServiceCollection services)
    {
      // Do nothing in base
      return services;
    }

    protected AppSettings InitializeSetting()
    {
      return new AppSettings
      {
        MailSettings = new MailSettings() { Server = "", Port = 25, From = "TestUser" },
      };
    }

    protected virtual FakeUnitTestUser InitializeUser()
    {
      return new FakeUnitTestUser()
      {
        Id = -1,
        AuthType = "UnitTestAuth",
        FirstName = "Test",
        LastName = "User",
        Level = 1,
        OrganizationUnit = "ELCA TEST",
        Principal = new ClaimsPrincipal(new ClaimsIdentity()),
        Rights = new List<Right>(),
        Site = "Universal",
        Username = "ELCANE\\TestUser",
        Visa = "WXW",
        TimeZoneId = Constants.TimeZoneId.DEFAULT
      };
    }

    protected void SetupRights(params Right[] rights)
    {
      UnitTestUser.Rights = rights;
    }

    protected void SetupLevel(int level)
    {
      UnitTestUser.Level = level;
    }

    protected void SetupUsername(string username)
    {
      UnitTestUser.Username = username;
    }

    protected void SetupUserId(int id)
    {
      UnitTestUser.Id = id;
    }

    // Tear down
    public virtual void Dispose()
    {
      UnitTestUser = null;
      Settings = null;
    }
  }

  public abstract class BaseTest<T> : BaseTest where T : class
  {
    protected BaseTest()
    {
      Service = Container.GetRequiredService<T>();
    }

    protected T Service { get; }
  }

  public abstract class BaseAuthorizerTest<T> : BaseTest where T : class
  {
    protected BaseAuthorizerTest()
    {
      Services = Container.GetRequiredService<IEnumerable<IAuthorizer<T>>>();
    }

    protected IEnumerable<IAuthorizer<T>> Services { get; }

    protected async Task<AuthorizationResult[]> AuthorizeAsync(T request)
    {
      return await Task.WhenAll(Services.Select(x => x.AuthorizeAsync(request)));
    }
  }

  public abstract class BaseValidatorTest<T> : BaseTest where T : class
  {
    protected BaseValidatorTest()
    {
      Services = Container.GetRequiredService<IEnumerable<IValidator<T>>>();
    }

    protected IEnumerable<IValidator<T>> Services { get; }

    protected async Task<ValidationFailure[]> ValidateAsync(T request)
    {
      var context = new ValidationContext<T>(request);

      var validationResults = await Task.WhenAll(Services.Select(v => v.ValidateAsync(context)));
      return validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToArray();
    }
  }

  public abstract class BaseHandlerTest<TRequest, TResponse> : BaseTest
    where TRequest : class, IRequest<TResponse>
  {
    protected BaseHandlerTest()
    {
      Service = Container.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
    }

    protected IRequestHandler<TRequest, TResponse> Service { get; }
  }
}

