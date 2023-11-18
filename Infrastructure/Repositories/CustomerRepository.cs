using Application.DTOs;
using Application.Persistence;
using Domain.Entitiy;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;


public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
{
    
    public CustomerRepository(CustomerContext dbContext) : base(dbContext)
    {
        // The base class (RepositoryBase) is initialized with the provided DbContext
    }


    // Check if the provided email is unique in the database
    public async Task<bool> IsEmailUnique(string email)
    {
        // Use the AllAsync method to check if there is no customer with the provided email
        return await _dbContext.Customers.AllAsync(customer => customer.Email != email);
    }

    // Check if the provided customer information (Name, DateOfBirth) is unique in the database
    public async Task<bool> IsCustomerUnique(CustomerDto customer)
    {
        // Use the AllAsync method to check if there is no customer with the same Name, DateOfBirth, and LastName
        return await _dbContext.Customers.AllAsync(l =>
            l.FirstName != customer.FirstName &&
            l.CreatedDate != customer.DateOfBirth &&
            l.LastName != customer.LastName
        );
    }
}
