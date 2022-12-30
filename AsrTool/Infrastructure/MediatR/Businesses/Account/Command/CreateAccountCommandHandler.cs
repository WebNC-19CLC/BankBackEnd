using AsrTool.Dtos;
using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AsrTool.Infrastructure.Exceptions;
using MediatR;
using BC = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;

namespace AsrTool.Infrastructure.MediatR.Businesses.Account.Command
{
  public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, BankAccountDto>
  {
    private readonly IAsrContext _asrContext;

    public CreateAccountCommandHandler(IAsrContext asrContext)
    {
      _asrContext = asrContext;
    }

    public async Task<BankAccountDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
      var ifUserNameExist = await _asrContext.Get<Employee>().AnyAsync(x => x.Username == request.Request.Username);
      if (ifUserNameExist)
      {
        throw new Exception("Username already exist");
      }

      var role = await _asrContext.Get<Domain.Entities.Role>().SingleOrDefaultAsync(x => x.Name == Constants.Roles.User);

      var password = GetRandomNumber();

      var newUser = new Employee
      {
        Username = request.Request.Username,
        Password = BC.HashPassword(password),
        Email = request.Request.Email,
        LastName = request.Request.LastName,
        Visa = request.Request.Username,
        FirstName = request.Request.FirstName,
        RoleId = role.Id,
        Active = true,
        Gender = Domain.Enums.Gender.Male,
        Phone = request.Request.Phone,
        Site = request.Request.Address,
        IdentityNumber = request.Request.IndentityNumber,
       
      };

     

  

      await _asrContext.AddRangeAsync(newUser);

      await _asrContext.SaveChangesAsync();

      var bankAccount = new BankAccount
      {
        AccountNumber = GetRandomNumber(),
        Balance = request.Request.Balance,
        Recipients = new List<Recipient>(),
        OTP = null,
        User = newUser
      };

      newUser.BankAccount = bankAccount;


      await _asrContext.AddRangeAsync(bankAccount);

      await _asrContext.SaveChangesAsync();

      var result = new BankAccountDto
      {
        Name = newUser.FullName,
        Username = newUser.Username,
        Password = password,
        AccountNumber = newUser.BankAccount.AccountNumber,
        Email = newUser.Email,
        Role = role.Name,
      };

      return result;
    }

    private string GetRandomNumber() {
      Random generator = new Random();
      String r = generator.Next(0, 1000000).ToString("D6");
      return r;
    }
  }
}
