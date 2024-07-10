namespace DomainMediator.DataBase;

public interface IDbSetStorage<Entity> where Entity : IDbEntity
{
    public Entity Add(Entity entity);
    public Task<Entity> AddAsync(Entity entity);
    public Entity AddAndSave(Entity entity);
    Task<Entity> AddAndSaveAsync(Entity entity);
    Entity? FindById(Guid id);
    Task<Entity?> FindByIdAsync(Guid id);
    void Update(Entity entity);
    Task UpdateAsync(Entity entity);
    void UpdateAndSave(Entity entity);
    Task UpdateAndSaveAsync(Entity entity);
    void DeleteById(Guid id);
    void DeleteByIdAndSave(Guid id);
    Task DeleteByIdAsync(Guid id);
    Task DeleteByIdAndSaveAsync(Guid id);
}
