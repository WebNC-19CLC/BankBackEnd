using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AsrTool.UnitTest._Common.Extensions
{
  internal static class ServiceCollectionExtensions
  {
    internal static IServiceCollection AddHandlers(this IServiceCollection services, Assembly assembly = null)
    {
      var usedAssembly = assembly ?? Assembly.GetExecutingAssembly();
      GetTypesAssignableTo(usedAssembly, typeof(IRequestHandler<,>)).ForEach((type) =>
      {
        foreach (var implementedInterface in type.ImplementedInterfaces)
        {
          services.AddScoped(implementedInterface, type);
        }
      });
      return services;
    }

    internal static IServiceCollection AddValidator(this IServiceCollection services, Assembly assembly = null)
    {
      var usedAssembly = assembly ?? Assembly.GetExecutingAssembly();
      GetTypesAssignableTo(usedAssembly, typeof(IValidator<>)).ForEach((type) =>
      {
        foreach (var implementedInterface in type.ImplementedInterfaces)
        {
          services.AddScoped(implementedInterface, type);
        }
      });
      return services;
    }

    private static List<TypeInfo> GetTypesAssignableTo(Assembly assembly, Type compareType)
    {
      return assembly.DefinedTypes.Where(x =>
        x.IsClass &&
        !x.IsAbstract &&
        x != compareType &&
        x.GetInterfaces()
          .Any(i =>
            i.IsGenericType &&
            i.GetGenericTypeDefinition() == compareType)).ToList();
    }

  }
}