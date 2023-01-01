using AsrTool.Infrastructure.Context;
using AsrTool.Infrastructure.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace AsrTool.Infrastructure.Filters
{
    public class BankSecurityService : IBankSecurityService
    {
        public void Secure(Bank bank, ActionExecutingContext context)
        {
            return;
        }
    }
}