 
using Application.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Commands.Customers.DeleteCustomer;

public class DeleteCustomerCommandHandler: IRequestHandler<DeleteCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteCustomerCommandHandler> _logger;

    public DeleteCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper, ILogger<DeleteCustomerCommandHandler> logger)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var customerToDelete = await _customerRepository.GetByIdAsync(request.Id);

            if (customerToDelete == null)
            {
                _logger.LogWarning($"Customer with ID {request.Id} not found. Deletion operation skipped.");
                return;
            }

            await _customerRepository.DeleteAsync(customerToDelete);
            _logger.LogInformation($"Customer {customerToDelete.Id} successfully deleted");
        }
        catch (System.Exception ex)
        {
            _logger.LogError($"An error occurred while deleting customer with ID {request.Id}: {ex.Message}", ex);
             throw;
        }
    }

}