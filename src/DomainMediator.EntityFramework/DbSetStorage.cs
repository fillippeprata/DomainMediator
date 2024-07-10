using DomainMediator.DataBase;
using Microsoft.EntityFrameworkCore;

namespace DomainMediator.EntityFramework;

public class DbSetStorage<Entity>(DomainDbContext _context) where Entity : class, IDbEntity
{
    protected DbSet<Entity> EntitySet => _context.Set<Entity>();

    public async Task<Entity> AddEntityAsync(Entity entity)
    {
        await _context.AddAsync(entity);
        return entity;
    }

    public async Task<Entity> AddEntityAndSaveAsync(Entity entity)
    {
        await AddEntityAsync(entity);
        await _context.CommitDomainChangesAsync();
        return entity;
    }

    public async Task<Entity?> FindEntityByIdAsync(Guid id) => await EntitySet.FirstOrDefaultAsync(x => x.Id == id);

    public Task UpdateEntityAsync(Entity entity)
    {
        EntitySet.Update(entity);
        return Task.CompletedTask;
    }

    public async Task UpdateEntityAndSaveAsync(Entity entity)
    {
        await UpdateEntityAsync(entity);
        await _context.CommitDomainChangesAsync();
    }

    public async Task DeleteEntityByIdAsync(Guid id)
    {
        var entity = await FindEntityByIdAsync(id);
        if (entity != null)
            EntitySet.Remove(entity);
    }

    public async Task DeleteByIdAndSaveAsync(Guid id)
    {
        await DeleteEntityByIdAsync(id);
        await _context.CommitDomainChangesAsync();
    }
}
