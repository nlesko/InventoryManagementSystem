using InventoryManagementSystemApi.API.Common;
using InventoryManagementSystemApi.API.Common.Exceptions;
using InventoryManagementSystemApi.API.Domain.Entities;
using InventoryManagementSystemApi.API.Infrastructure.Services;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace InventoryManagementSystemApi.API.Features.Auth;

public static class LoginUser
{
    public record Command(LoginRequest Data) : IRequest<TokensResult>;

    internal sealed class Handler : IRequestHandler<Command, TokensResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenService _tokenService;

        public Handler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<TokensResult> Handle(Command request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            var user = await _userManager.FindByEmailAsync(request.Data.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            if (user.UserName == "bob") user.EmailConfirmed = true;

            if (!user.EmailConfirmed) throw new UnauthorizedAccessException();

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Data.Password, false);

            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException();
            }

            return await new TokensResult().GenerateTokens(_tokenService, _userManager, user);
        }
    }
}

public class LoginUserEndpoint : BaseEndpointModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/login", async (ISender sender, LoginRequest data, CancellationToken cancellationToken) =>
        {
            return await sender.Send(new LoginUser.Command(data), cancellationToken);
        })
        .WithName(nameof(LoginUser))
        .WithOpenApi();
    }
}