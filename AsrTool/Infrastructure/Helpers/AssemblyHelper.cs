using System.Reflection;

namespace AsrTool.Infrastructure.Helpers
{
  public static class AssemblyHelper
  {
    public static byte[] GetResource(string resourceName, Assembly assembly)
    {
      using var stream = GetResourceStream(resourceName, assembly);
      using var memoryStream = new MemoryStream();
      stream.CopyTo(memoryStream);
      return memoryStream.ToArray();
    }

    public static Stream GetResourceStream(string resourceName, Assembly assembly)
    {
      assembly ??= Assembly.GetExecutingAssembly();
      var resourceRealName = assembly.GetManifestResourceNames().Single(x => x.Contains(resourceName));
      return assembly.GetManifestResourceStream(resourceRealName) ?? throw new InvalidDataException();
    }

    public static IEnumerable<T> GetImplementationsOfTypeInSolution<T>()
    {
      return GetImplementationsOfTypeInSolution<T>(GetSolutionAssemblies());
    }

    public static IEnumerable<T> GetImplementationsOfTypeInSolution<T>(params Assembly[] assemblies)
    {
      var outType = typeof(T);
      return assemblies
          .SelectMany(x => x!.GetTypes())
          .Where(x => outType.IsAssignableFrom(x) && x.IsClass && !x.IsAbstract)
          .Select(x => (T)Activator.CreateInstance(x)!)
          .Where(x => x != null)
          .ToList()!;
    }

    public static Assembly[] GetSolutionAssemblies()
    {
      var searchPattern = $"{nameof(AsrTool)}.*.dll";
      var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, searchPattern)
          .Select(TryLoadAssemblyByPath).Where(x => x != null);
      return assemblies.ToArray()!;
    }

    private static Assembly? TryLoadAssemblyByPath(string path)
    {
      try
      {
        return Assembly.Load(AssemblyName.GetAssemblyName(path));
      }
      catch (Exception)
      {
        // just ignore the assembly that we can not loaded.
        return null;
      }
    }
  }
}
