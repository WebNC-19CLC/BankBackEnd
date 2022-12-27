using BC = BCrypt.Net.BCrypt;
using System.Security;
using System.Security.Claims;
using System.Security.Principal;
using AsrTool.Infrastructure.Common;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Objects.Configurations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using AsrTool.Dtos;
using AsrTool.Infrastructure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AsrTool.Infrastructure.Exceptions;

namespace AsrTool.Infrastructure.Auth
{
  public class UserManager : IUserManager
  {
    private readonly ILogger<UserManager> _logger;
    private readonly AppSettings _settings;
    private readonly ICacheService _cache;
    private readonly ILdapConnector _ldapConnector;
    private readonly IAsrContext _context;

    public UserManager(
      ILogger<UserManager> logger, IOptions<AppSettings> settings, ICacheService cache, ILdapConnector ldapConnector, IAsrContext context)
    {
      _logger = logger;
      _cache = cache;
      _ldapConnector = ldapConnector;
      _settings = settings.Value;
      _context = context;
    }

    public async Task SignIn(HttpContext httpContext, string username, string password, bool isPersistent = false)
    {
      _logger.LogInformation("--------------> Sign in start <----------------");
      try
      {
        var user = await _context.Get<Employee>().SingleOrDefaultAsync(x => x.Username == username);
        
        if (user == null) {
          throw new UnauthorizerException("Username is not correct");  
        }

        bool matchPassword = BC.Verify(password, user.Password);

        if (!matchPassword) {
          throw new UnauthorizerException("Password is not correct");
        }

        if (matchPassword)
        {
          var domainUserName = $"{username}";
          _logger.LogInformation($"--------------> Signed in as {domainUserName} <----------------");
          var detail = await ReceiveDetail(domainUserName);
          var authenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
          var claimPrincipal = CreatePrincipal(detail, authenticationScheme, false);
          var authProperties = new AuthenticationProperties
          {
           
          };

          await httpContext.SignInAsync(authenticationScheme, claimPrincipal, authProperties);
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "--------------> Signed in failed <----------------");
        throw;
      }
      finally
      {
        _logger.LogInformation("--------------> Signed in End <----------------");
      }
    }

    private static ClaimsPrincipal CreatePrincipal(ShortUserDetail detail, string authenticationType, bool isWindowAuth = false)
    {
      var usedClaims = new List<Claim>();
      usedClaims.Add(new Claim(ClaimTypes.NameIdentifier, detail.Identifier));
      usedClaims.Add(new Claim(ClaimTypes.Name, detail.Name));
      usedClaims.Add(new Claim(ClaimTypes.Email, detail.Email));
      usedClaims.Add(new Claim(Constants.Auth.AUTHENTICATION_CLAIMS_TYPE, isWindowAuth ? Constants.Auth.WINDOW_AUTHENTICATION : Constants.Auth.BASIC_AUTHENTICATION));

      var outputIdentity = new ClaimsIdentity(usedClaims, authenticationType);
      return new ClaimsPrincipal(outputIdentity);
    }

    private async Task<ICollection<Claim>> ReceiveClaims(string username)
    {
      var user = await _cache.GetEmployeeCachingItem($"{username}".ToUpperInvariant());
      return user.Rights?.Select(x => new Claim(Constants.Auth.CLAIM_APP_NAME, x.ToString())).ToList() ?? new List<Claim>();
    }

    private async Task<ShortUserDetail> ReceiveDetail(string username)
    {
      var user = await _cache.GetEmployeeCachingItem($"{username}".ToUpperInvariant());
      return new ShortUserDetail
      {
        Email = user.Email,
        Identifier = username,
        Name = username
      };
    }

    public async Task SignOut(HttpContext httpContext)
    {
      await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public async Task GrantPermissions(HttpContext httpContext)
    {
      var username = httpContext.User.Identity?.Name;
      if (string.IsNullOrWhiteSpace(username))
      {
        throw new SecurityException("Forbidden");
      }

      var receiveClaims = await ReceiveClaims(username);
      var identity = (ClaimsIdentity)httpContext.User.Identity!;
      identity.AddClaims(receiveClaims);
    }

    public async Task Register(HttpContext httpContext, RegisterRequestDto model)
    {
      var ifUserNameExist = await _context.Get<Employee>().AnyAsync(x => x.Username == model.Username);
      if (ifUserNameExist) {
        throw new Exception("Username is already existed");
      }

      var userRole = await _context.Get<Role>().SingleOrDefaultAsync(x => x.Name == Constants.Roles.User);

      var newUser = new Employee
      {
        Username = model.Username,
        Password = BC.HashPassword(model.Password),
        Email = model.Email,
        LastName = model.LastName,
        Visa = model.Username,
        FirstName = model.FirstName,
        Active = true,
        Gender = Domain.Enums.Gender.Male,
        Phone = model.Phone,
        Site = model.Address,
        RoleId  = userRole.Id,
        IdentityNumber = model.IndentityNumber,
        CreatedBy = "System",
        CreatedOn = DateTime.UtcNow,
        ModifiedBy = "System",
        ModifiedOn = DateTime.UtcNow,
      };

      await _context.AddAsync(newUser);
      await _context.SaveChangesAsync();

      await SignIn(httpContext, model.Username, model.Password);
    }

    public async Task RegisterAdmin(HttpContext httpContext, RegisterRequestDto model)
    {
      var ifUserNameExist = await _context.Get<Employee>().AnyAsync(x => x.Username == model.Username);
      if (ifUserNameExist)
      {
        throw new Exception("Username is already existed");
      }

      var adminRole = await _context.Get<Role>().SingleOrDefaultAsync(x => x.Name == Constants.Roles.Admin);

      var newUser = new Employee
      {
        Username = model.Username,
        Password = BC.HashPassword(model.Password),
        Email = model.Email,
        LastName = model.LastName,
        Visa = model.Username,
        FirstName = model.FirstName,
        Active = true,
        Gender = Domain.Enums.Gender.Male,
        Phone = model.Phone,
        Site = model.Address,
        RoleId = adminRole.Id,
        IdentityNumber = model.IndentityNumber,
        CreatedBy = "System",
        CreatedOn = DateTime.UtcNow,
        ModifiedBy = "System",
        ModifiedOn = DateTime.UtcNow,
      };

      await _context.AddAsync(newUser);
      await _context.SaveChangesAsync();

      await SignIn(httpContext, model.Username, model.Password);
    }

    private class ShortUserDetail
    {
      public string Identifier { get; set; }

      public string Name { get; set; }

      public string Email { get; set; }
    }
  }
}