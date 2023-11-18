using Domain.Entitiy;
using MediatR;

namespace Application.Features.Queries.GetCustomerList;

public class GetCustomerListQuery: IRequest<IReadOnlyList<Customer>>
{
    
}