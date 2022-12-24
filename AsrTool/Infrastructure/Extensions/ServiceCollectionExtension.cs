using System.Reflection;
using AsrTool.Infrastructure.MediatR.Common.Interfaces;

namespace AsrTool.Infrastructure.Extensions
{
  public static class ServiceCollectionExtension
  {
    public static IServiceCollection AddAuthorizers(this IServiceCollection services, Assembly assembly = null)
    {
      var usedAssembly = assembly ?? Assembly.GetExecutingAssembly();
      GetTypesAssignableTo(usedAssembly, typeof(IAuthorizer<>)).ForEach((type) =>
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