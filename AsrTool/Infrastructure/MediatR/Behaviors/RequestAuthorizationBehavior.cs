using System.Security;
using AsrTool.Infrastructure.MediatR.Common.Interfaces;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AsrTool.Infrastructure.MediatR.Behaviors
{
  public class RequestAuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
  {
    private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
    {
      Converters = new List<JsonConverter> { new StringEnumConverter() },
      NullValueHandling = NullValueHandling.Ignore,
      PreserveReferencesHandling = PreserveReferencesHandling.Objects,
      TypeNameHandling = TypeNameHandling.Objects,
      ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
      Formatting = Formatting.Indented
    };

    private readonly IEnumerable<IAuthorizer<TRequest>> _authorizers;

    public RequestAuthorizationBehavior(IEnumerable<IAuthorizer<TRequest>> authorizers)
    {
      _authorizers = authorizers;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
      foreach (var authorizer in _authorizers)
      {
        var result = await authorizer.AuthorizeAsync(request, cancellationToken);
        if (!result.Succeeded)
        {
          throw new SecurityException($"Forbidden: {Environment.NewLine}{JsonConvert.SerializeObject(request, JsonSerializerSettings)}");
        }
      }

      return await next();
    }
  }
}