namespace DomainMediator.DataBase;

public interface IDbUnitOfWork
{
    public Task CommitDomainChangesAsync();
}