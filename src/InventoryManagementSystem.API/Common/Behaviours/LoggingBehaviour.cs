using InventoryManagementSystem.API.Common.Interfaces;

using MediatR.Pipeline;

namespace InventoryManagementSystem.API.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly ICurrentUserService _currentUserService;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.UserId.HasValue ? _currentUserService.UserId.Value.ToString() : string.Empty;

        return Task.Run(
            () => _logger.LogInformation(
            "VerticalSlice Request: {Name} {@UserId} {@Request}",
            requestName,
            userId,
            request),
            cancellationToken);
    }
}