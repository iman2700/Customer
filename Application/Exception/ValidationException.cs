using FluentValidation.Results;

namespace Application.Exception;

// Custom exception class for handling validation failures
public class ValidationException : ApplicationException
{
    // Property to store validation errors in a dictionary format
    public IDictionary<string, string[]> Errors { get; }
    public ValidationException() : base("One or more validation failures have occurred.")
    {
        // Initialize the Errors dictionary
        Errors = new Dictionary<string, string[]>();
    }

    // Constructor with a collection of ValidationFailure objects
    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        // Convert the collection of ValidationFailure objects to a dictionary
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(
                failuresGroup => failuresGroup.Key,
                failureGroup => failureGroup.ToArray()
            );
    }

    
   
}
