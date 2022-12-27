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

      var password = GetRandomNumber();

      var newUser = new Employee
      {
        Username = request.Request.Username,
        Password = BC.HashPassword(password),
        Email = request.Request.Email,
        LastName = request.Request.LastName,
        Visa = request.Request.Username,
        FirstName = request.Request.FirstName,
        Active = true,
        Gender = Domain.Enums.Gender.Male,
        Phone = request.Request.Phone,
        Site = request.Request.Address,
        IdentityNumber = request.Request.IndentityNumber,
        BankAccount = new BankAccount {
          AccountNumber = GetRandomNumber(),
          Balance = request.Request.Balance,
          Recipients = new List<Recipient>(),
          OTPS = new List<OTP>(),
        }
      };

      await _asrContext.AddAsync(newUser);

      await _asrContext.SaveChangesAsync();

      var result = new BankAccountDto
      {
        Name = newUser.FullName,
        Username = newUser.Username,
        Password = password,
        AccountNumber = newUser.BankAccount.AccountNumber,
        Email = newUser.Email
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
