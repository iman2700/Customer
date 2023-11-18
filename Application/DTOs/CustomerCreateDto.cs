namespace Application.DTOs;

public record CustomerCreateDto(string FirstName, string LastName, DateTime DateOfBirth, string PhoneNumber, string BankAccountNumber);

