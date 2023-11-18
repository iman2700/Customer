using Application.Persistence;
using AutoMapper;
using Domain.Entitiy;
using MediatR;

namespace Application.Features.Queries.GetCustomerList;

public class GetCustomerListQueryHandler : IRequestHandler<GetCustomerListQuery, IReadOnlyList<Customer>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomerListQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<Customer>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
    {
        var customerList = await _customerRepository.GetAllAsync();
        return _mapper.Map<IReadOnlyList<Customer>>(customerList);
    }
}