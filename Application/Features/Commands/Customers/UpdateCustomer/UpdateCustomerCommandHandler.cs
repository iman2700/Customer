using Application.Exception;
using Application.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Commands.Customers.UpdateCustomer;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateCustomerCommandHandler> _logger;
    private readonly UpdateCustomerCommandValidator _validator;


    public UpdateCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper, ILogger<UpdateCustomerCommandHandler> logger, UpdateCustomerCommandValidator validator)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _validator = validator;
    }

    public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {

        // Validate the command using the validator
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            // Log validation errors
            foreach (var error in validationResult.Errors)
            {
                _logger.LogError($"Validation error: {error.ErrorMessage}");
            }

            // Throw a validation exception
            throw new ValidationException(validationResult.Errors);
        }




        // Retrieve the customer to update from the repository based on the request ID
        var customerToUpdate = await _customerRepository.GetByIdAsync(request.Id);

        if (customerToUpdate == null)
        {
            // Log a warning if the customer is not found and skip the update operation
            _logger.LogWarning($"Customer with ID {request.Id} not found. Update operation skipped.");
            return;
        }

        try
        {
             _mapper.Map(request, customerToUpdate);

            await _customerRepository.UpdateAsync(customerToUpdate);

             _logger.LogInformation($"Customer {customerToUpdate.Id} successfully updated");
        }
        catch (System.Exception ex)
        {
             
            _logger.LogError($"An error occurred while updating customer with ID {request.Id}: {ex.Message}", ex);
            throw;
        }
    }


}