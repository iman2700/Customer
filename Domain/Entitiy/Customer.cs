using Domain.Common;

namespace Domain.Entitiy;

public class Customer: EntityBase
{

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public string BankAccountNumber { get; set; }
}