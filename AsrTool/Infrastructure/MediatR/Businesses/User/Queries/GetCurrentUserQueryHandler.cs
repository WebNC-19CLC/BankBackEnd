using AsrTool.Dtos;
using AsrTool.Infrastructure.Auth;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;

namespace AsrTool.Infrastructure.MediatR.Businesses.User.Queries
{
  public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
  {
    private readonly IUserResolver _userResolver;
    private readonly IMapper _mapper;
    private readonly IAntiforgery _antiforgery;

    public GetCurrentUserQueryHandler(IUserResolver userResolver, IMapper mapper, IAntiforgery antiforgery)
    {
      _userResolver = userResolver;
      _mapper = mapper;
      _antiforgery = antiforgery;
    }

    public Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
      var userDto = _mapper.Map<IUser, UserDto>(_userResolver.CurrentUser);
      userDto.XsrfToken = SetAntiForgery(request.HttpContext, request.HttpResponse);
      return Task.FromResult(userDto);
    }

    private string SetAntiForgery(HttpContext httpContext, HttpResponse response)
    {
      var tokens = _antiforgery.GetAndStoreTokens(httpContext);
      response.Cookies.Append(Constants.Auth.ANTIFORGERY_HEADER, tokens.RequestToken, new CookieOptions { HttpOnly = false });
      return tokens.RequestToken;
    }
  }
}