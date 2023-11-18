
using Application.DTOs;
using Domain.Entitiy;

namespace Application.Persistence;

public interface ICustomerRepository:IAsyncRepository<Customer>
{
    Task<bool> IsEmailUnique(string emailAddress);
    Task<bool> IsCustomerUnique(CustomerDto customer);
}