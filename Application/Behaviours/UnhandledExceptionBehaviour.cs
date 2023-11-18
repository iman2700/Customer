using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviours;
/// <summary>
/// Pipeline behavior to handle unhandled exceptions that may occur during the processing of a MediatR request.
/// </summary>
 
public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnhandledExceptionBehaviour{TRequest, TResponse}"/> class.
    /// </summary>
 
    public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the MediatR request and delegates to the next behavior in the pipeline.
    /// Catches any unhandled exceptions and logs them before rethrowing.
    /// </summary>
 
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            // Continue with the next behavior in the pipeline
            return await next();
        }
        catch (System.Exception ex)
        {
            // Log unhandled exception with request details
            var requestName = typeof(TRequest).Name;
            _logger.LogError(ex, "Unhandled exception for application request: {name} {@Request}", requestName, request);
            
            // Rethrow the exception after logging
            throw;
        }
    }
}
