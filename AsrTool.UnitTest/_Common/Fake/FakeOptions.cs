using Microsoft.Extensions.Options;

namespace AsrTool.UnitTest._Common.Fake
{
  public class FakeOptions<T> : IOptions<T> where T : class, new()
  {
    private readonly T _config;

    public FakeOptions(T config)
    {
      _config = config;
    }

    public T Value => _config;
  }
}