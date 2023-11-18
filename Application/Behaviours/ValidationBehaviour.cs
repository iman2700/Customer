using FluentValidation;
using MediatR;
using ValidationException = Application.Exception.ValidationException;

namespace Application.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) 
    {
        // Perform validation only if there are registered validators for the request type
        if (_validators.Any()) 
        {
            // Create a validation context for the request
            var context = new ValidationContext<TRequest>(request);
            
            // Execute validation on the request using all registered validators
            var validationResult = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            
            // Get all validation failures from the validation results
            var failures = validationResult.SelectMany(r => r.Errors).Where(f => f != null).ToList();
            
            // If there are validation failures, throw a ValidationException
            if (failures.Count != 0) 
            {
                throw new ValidationException(failures);
            }
        }
        
        // If validation is successful, call the next handler in the pipeline
        return await next();
    }
}