using InventoryManagementSystemApi.API.Common;
using InventoryManagementSystemApi.API.Domain.Entities;
using InventoryManagementSystemApi.API.Infrastructure.Services;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace InventoryManagementSystemApi.API.Features.Auth;

public static class RegisterUser
{
    public record Command(RegisterRequest Data) : IRequest<RegisterResult>;

    internal sealed class Handler : IRequestHandler<Command, RegisterResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenService _tokenService;

        public Handler(UserManager<ApplicationUser> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<RegisterResult> Handle(Command request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            // if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            // {
            //     ModelState.AddModelError("email", "Email taken");
            //     return ValidationProblem();
            // }
            // if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
            // {
            //     ModelState.AddModelError("username", "Username taken");
            //     return ValidationProblem();
            // }

            var user = new ApplicationUser
            {
                Email = request.Data.Email,
                UserName = request.Data.Email.Split('@')[0],
                FirstName = request.Data.FirstName,
                LastName = request.Data.LastName
            };

            var result = await _userManager.CreateAsync(user, request.Data.Password);

            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException();
            }

            // var origin = Request.Headers["origin"];
            // var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            // token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            // var verifyUrl = $"{origin}/account/verifyEmail?token={token}&email={user.Email}";
            // var message = $"<p>Please click the below link to verify your email address:</p><p><a href='{verifyUrl}'>Click to verify email</a></p>";

            // await _emailSender.SendEmailAsync(user.Email, "Please verify email", message);

            return new RegisterResult {
                IsSuccessful = true
            };
        }       
    }
}

public class RegisterUserEndpoint : BaseEndpointModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/accounts/register", async (ISender sender, RegisterRequest data, CancellationToken cancellationToken) =>
        {
            return await sender.Send(new RegisterUser.Command(data), cancellationToken);
        })
        .WithName(nameof(RegisterUser))
        .WithOpenApi();
    }
}