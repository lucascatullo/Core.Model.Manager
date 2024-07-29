
using Code.Models.Manager.Builder;
using Code.Models.Manager.Model;
using Core.Models.Manager.Model;
using Microsoft.EntityFrameworkCore;

namespace Code.Models.Manager.Service;

public abstract class ArchiveModelService<T, TKey> : BaseService<T, TKey> where T : notnull, BaseModel<TKey>, IArchiveModel where TKey : notnull
{
    protected new IArchiveModelQueryBuilder<T, TKey> _queryBuilder;

    public ArchiveModelService(DbContext db) : base(db)
    {
        _queryBuilder = (StartQuery() as IArchiveModelQueryBuilder<T, TKey>)!;
    }
    /// <summary>
    /// Changes the Archive property for a quanttiy of antities that are older thahn the days property
    /// </summary>
    /// <param name="quantity">Quantity of properties</param>
    /// <param name="days">Days old</param>
    public async Task BatchArchiveEntities(int quantity, int days)
    {
        var entities = await _queryBuilder.IgnoreArchived().MetArchiveConditions(days).ExecuteAsync(quantity);

        foreach (var entity in entities)
        {
            dataBaseObj = entity;
            MakeReadyToArchive();
        }
        await _db.SaveChangesAsync();
    }
    /// <summary>
    /// Changes the Archive property for a quanttiy of antities that met the condition function
    /// </summary>
    /// <param name="quantity">quantity of entities</param>
    /// <param name="condition">Condition function</param>
    public async Task BatchArchiveEntities(int quantity, Func<T, bool> condition)
    {
        var entities = await _queryBuilder.IgnoreArchived().MetArchiveConditions(condition).ExecuteAsync(quantity);

        foreach (var entity in entities)
        {
            dataBaseObj = entity;
            MakeReadyToArchive();
        }
        await _db.SaveChangesAsync();
    }
}
