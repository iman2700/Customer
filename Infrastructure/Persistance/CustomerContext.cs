using Domain.Common;
using Domain.Entitiy;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance;

public class CustomerContext: DbContext
{
    public CustomerContext(DbContextOptions<CustomerContext> option) : base(option) { }

    public DbSet<Customer> Customers { get; set; }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<EntityBase>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.Now;
                    entry.Entity.CreatedBy = "iman";
                    entry.Entity.LastModifiedBy = "iman";
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedDate = DateTime.Now;
                    entry.Entity.LastModifiedBy = "iman";
                    break;

            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}