using DomainMediator.DataBase;
using Microsoft.EntityFrameworkCore;

namespace DomainMediator.EntityFramework;

public class DomainDbContext(DbContextOptions<DomainDbContext> options)
    : DbContext(options), IDbUnitOfWork
{
    public async Task CommitDomainChangesAsync()
    {
        await SaveChangesAsync();
    }
}