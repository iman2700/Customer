using Application.Persistence;
using AutoMapper;
using Domain.Entitiy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Commands.Customers.CreateCustomer;

public class CreateCustomerCommandHandler:IRequestHandler<CreateCustomerCommand, int>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCustomerCommandHandler> _logger;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper, ILogger<CreateCustomerCommandHandler> logger)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
            
        var customerrEntity = _mapper.Map<Customer>(request);
        var newCustomer = await _customerRepository.AddAsync(customerrEntity);
        return newCustomer.Id;
    }
         
}