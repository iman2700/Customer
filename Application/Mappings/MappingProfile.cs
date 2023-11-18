using Application.DTOs;
using Application.Features.Commands.Customers.CreateCustomer;
using Application.Features.Commands.Customers.UpdateCustomer;
using Application.Features.Queries.GetCustomer;
using AutoMapper;
using Domain.Entitiy;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CreateCustomerCommand>().ReverseMap();
        CreateMap<Customer, UpdateCustomerCommand>().ReverseMap();
        CreateMap<Customer, CustomerCreateDto>().ReverseMap();
        CreateMap<Customer, GetCustomerQuery>().ReverseMap();
        CreateMap<CustomerDto, CreateCustomerCommand>().ReverseMap();
        CreateMap<CustomerDto, UpdateCustomerCommand>().ReverseMap();
    }
}