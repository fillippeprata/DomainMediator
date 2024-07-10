using DomainMediator.EntityFramework.Example.Users;
using Microsoft.EntityFrameworkCore;

namespace DomainMediator.EntityFramework.Example;

public class ExampleContextDbContext(DbContextOptions<DomainDbContext> options) : DomainDbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
}