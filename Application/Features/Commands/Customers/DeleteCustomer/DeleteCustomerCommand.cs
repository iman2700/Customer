using MediatR;

namespace Application.Features.Commands.Customers.DeleteCustomer;

public class DeleteCustomerCommand: IRequest
{
    public int Id { get; set; }
}