using Application.DTOs;
using Application.Persistence;
using AutoMapper;
using FluentValidation;
using libphonenumber;

namespace Application.Features.Commands.Customers.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    // Constructor with dependency injection
    public CreateCustomerCommandValidator(ICustomerRepository customerRepository, IMapper mapper)
    {
        // Ensure dependencies are not null, throw ArgumentNullException if they are
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        // Configure validation rules
        ConfigureValidationRules();
    }

    // Method to configure validation rules
    private void ConfigureValidationRules()
    {
        // Validation rule for FirstName
        RuleFor(p => p.FirstName)
            .NotEmpty().WithMessage("{Name} is required.")
            .NotNull()
            .MaximumLength(50).WithMessage("{Name} must not exceed 50 characters.");

        // Validation rule for LastName
        RuleFor(p => p.LastName)
            .NotEmpty().WithMessage("{LastName} is required.")
            .NotNull()
            .MaximumLength(50).WithMessage("{LastName} must not exceed 50 characters.");

        // Validation rule for Email
        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("{Email Address} is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MustAsync(UniqueEmail).WithMessage("Email already taken");

        // Validation rule for PhoneNumber
        RuleFor(p => p.PhoneNumber)
            .Must(IsValidNumber).WithMessage("Phone Number is not valid");

        // Validation rule for the entire command, checking uniqueness based on Name, LastName, and DateOfBirth
        RuleFor(p => p)
            .MustAsync(UniqueCustomer)
            .WithMessage("Customer with the same Name, LastName, and DateOfBirth already registered.");
    }

    // Method to check if a customer is unique based on certain criteria
    private async Task<bool> UniqueCustomer(CreateCustomerCommand customer, CancellationToken cancellationToken)
    {
        var customerEntity = _mapper.Map<CustomerDto>(customer);
        return await _customerRepository.IsCustomerUnique(customerEntity);
    }

    // Method to check if an email is unique
    private async Task<bool> UniqueEmail(string email, CancellationToken cancellationToken)
    {
        return await _customerRepository.IsEmailUnique(email);
    }

    // Method to check if a phone number is valid
    private bool IsValidNumber(string phoneNumber)
    {
        phoneNumber = phoneNumber.Trim();

        if (phoneNumber.StartsWith("00"))
        {
            // Replace 00 at the beginning with +
            phoneNumber = "+" + phoneNumber[2..];
        }

        try
        {
            // Use the PhoneNumberUtil library to parse and validate the phone number
            return PhoneNumberUtil.Instance.Parse(phoneNumber, "").IsValidNumber;
        }
        catch
        {
            // Exception means it's not a valid number
            return false;
        }
    }
}
