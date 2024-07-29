

using Core.Models.Manager.DataManager;
using Core.Models.Manager.Interface;
using Core.Models.Manager.Model;
using Core.Models.Manager.Strategy;
using Microsoft.EntityFrameworkCore;

namespace Code.Models.Manager.Service;

public abstract class BaseService<T, TKey> : BaseDataManager<T, TKey>, IBaseService<T,TKey> where T : notnull, BaseModel<TKey> where TKey : notnull
{
    /// <summary>
    /// Main Context.
    /// </summary>
    private readonly DbContext _db;

    /// <summary>
    /// Used to query for objects.
    /// </summary>
    protected readonly IQueryBuilder<T, TKey> _queryBuilder;


    public BaseService(DbContext db)
    {
        _db = db;

        _queryBuilder = StartQuery();
    }

    public abstract IQueryBuilder<T, TKey> StartQuery();
    /// <summary>
    /// Defualt strategy, allways returns true
    /// </summary>
    public IDeleteStrategy<T, TKey> deleteStrategy = new NullDeleteStrategy<T, TKey>();


    /// <summary>
    /// If the delete strategy conditions are met, the entity will be logical deleted.
    /// </summary>
    /// <param name="id">Id of the Deleted entity</param>
    /// <returns>True if the entity was deleted correctly.</returns>
    public async Task<bool> LogicalDeleteAsync(TKey id)
    {
        dataBaseObj = await _queryBuilder.GetAsync(id);

        Delete(deleteStrategy);

        await _db.SaveChangesAsync();
        return true;
    }


    /// <summary>
    /// If the delete strategy conditions are met, the entity will be logical deleted.
    /// </summary>
    /// <param name="id">Id of the deleted entity </param>
    /// <param name="userId">id of the current user.</param>
    /// <param name="userRoles">Roles of the current user.</param>
    /// <returns>True if the delete was succesfull</returns>
    public async Task<bool> LogicalDeleteAsync(TKey id, string? userId, string? userRoles)
    {
        dataBaseObj = await _queryBuilder.GetAsync(id);

        Delete(deleteStrategy, userId, userRoles);

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<T> GetAsync(TKey id) => await _queryBuilder.GetAsync(id);

}
