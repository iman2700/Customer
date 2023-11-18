using Domain.Entitiy;
using MediatR;

namespace Application.Features.Queries.GetCustomer;

public class GetCustomerQuery: IRequest<Customer>
{
    public int customerId { get; set; }
    public GetCustomerQuery(int id)
    {
        customerId = id;
    }
}