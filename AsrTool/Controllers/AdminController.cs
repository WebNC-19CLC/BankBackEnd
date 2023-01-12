using AsrTool.Dtos;
using AsrTool.Dtos.UserRoleDtos;
using AsrTool.Infrastructure.MediatR.Businesses.Account.Command;
using AsrTool.Infrastructure.MediatR.Businesses.Role.Commands;
using AsrTool.Infrastructure.MediatR.Businesses.User.Commands;
using AsrTool.Infrastructure.MediatR.Businesses.User.Queries;
using AsrTool.Infrastructure.MediatR.Businesses.Admin.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AsrTool.Infrastructure.MediatR.Businesses.Admin.Queries;
using AsrTool.Swagger.Admin;
using Swashbuckle.AspNetCore.Filters;

namespace AsrTool.Controllers
{
  [Authorize]
  public class AdminController : BaseApiController
  {
    public AdminController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Get all employees
    /// </summary>
    /// <returns>A list of Employees</returns>
    /// <response code="400">Bad request</response>
    [SwaggerResponseExample(200, typeof(EmployeesExample))]
    [HttpGet("employee")]
    public async Task<ICollection<EmployeeDto>> GetEmployees()
    {
      return await Mediator.Send(new GetEmployeeQuery());
    }


    /// <summary>
    /// Update an employee
    /// </summary>
    /// <returns>Employee has been updated</returns>
    /// <response code="400">Bad request</response>
    [SwaggerResponseExample(200, typeof(EmployeeExample))]
    [SwaggerRequestExample(typeof(EmployeeDto), typeof(EmployeeExample))]
    [HttpPut("employee")]
    public async Task<EmployeeDto> UpdateEmployee([FromBody] EmployeeDto dto)
    {
      return await Mediator.Send(new UpdateEmployeeCommand() {Request = dto });
    }

    /// <summary>
    /// Delete an employee
    /// </summary>
    /// <returns>Status</returns>
    [HttpDelete("employee/{id}")]
    public async Task DeleteEmployee([FromRoute] int id)
    {
      await Mediator.Send(new DeleteEmployeeCommnad() { Id = id });
    }

    /// <summary>
    /// Create bank account for user
    /// </summary>
    /// <returns>Bank account</returns>
    /// <response code="400">Bad request</response>
    [SwaggerResponseExample(200, typeof(BankAccountDtoExample))]
    [SwaggerRequestExample(typeof(CreateAccountDto), typeof(CreateAccountDtoExample))]
    [HttpPost("create-bank-account")]
    public async Task<BankAccountDto> CreateBankAccount([FromBody] CreateAccountDto request)
    {
      return await Mediator.Send(new CreateAccountCommand { Request = request });
    }


    /// <summary>
    /// Create bank account for employee
    /// </summary>
    /// <returns>Employee account</returns>
    /// <response code="400">Bad request</response>
    [SwaggerResponseExample(200, typeof(BankAccountDtoExample))]
    [SwaggerRequestExample(typeof(CreateAccountDto), typeof(CreateAccountDtoExample))]
    [HttpPost("create-employee-account")]
    public async Task<BankAccountDto> CreateEmployeeAccount([FromBody] CreateAccountDto request)
    {
      return await Mediator.Send(new CreateEmployeeAccountCommand { Request = request });
    }

    /// <summary>
    /// Set user's active status
    /// </summary>
    /// <response code="400">Bad request</response>
    [SwaggerRequestExample(typeof(SetActiveStatusDto), typeof(SetActiveStatusDtoExample))]
    [HttpPost("set-user-active-status")]
    public async Task SetActiveStatus([FromBody] SetActiveStatusDto request)
    {
      await Mediator.Send(new SetUserActiveStatusCommand { Request = request });
    }

    /// <summary>
    /// List transaction for admin
    /// </summary>
    /// <returns>Statitic of transactions and list of transactions</returns>
    /// <response code="400">Bad request</response>
    [SwaggerResponseExample(200, typeof(AdminListTransactionDtoExample))]
    [SwaggerRequestExample(typeof(ListTransactionFilter), typeof(ListTransactionFilterExample))]
    [HttpPost("list-transactions")]
    public async Task<AdminListTransactionDto> ListTransactions([FromBody] ListTransactionFilter filter)
    {
      return await Mediator.Send(new ListTransactionQuery { Filter = filter });
    }
  }
}
