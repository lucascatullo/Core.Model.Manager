

using Code.Models.Manager.Model;
using Core.Models.Manager.Builder;
using Core.Models.Manager.Model;

namespace Code.Models.Manager.Builder;

public class ArchiveModelQueryBuilder<T, TKey> : QueryBuilder<T, TKey>, IArchiveModelQueryBuilder<T, TKey> where T : notnull, BaseModel<TKey>, IArchiveModel where TKey : notnull
{
    public ArchiveModelQueryBuilder(IQueryable<T> initialQuery) : base(initialQuery)
    {
    }

    /// <summary>
    /// Querys will only return unarchived entities
    /// </summary>
    /// <returns>This</returns>
    public IArchiveModelQueryBuilder<T, TKey> IgnoreArchived()
    {
        _query = _query.Where(x => !x.IsArchived);
        return this;
    }

    /// <summary>
    /// Query will return only entites older then this number of days.
    /// </summary>
    /// <param name="days">Number of days</param>
    /// <returns>this</returns>
    public IArchiveModelQueryBuilder<T, TKey> MetArchiveConditions(int days)
    {
        _query = _query.Where(x => x.CreatedDate.AddDays(days) < DateTime.UtcNow);
        return this;
    }

    /// <summary>
    /// Query will only return entities that met this condition.
    /// </summary>
    /// <param name="condition">Quqery condition</param>
    /// <returns>this</returns>
    public IArchiveModelQueryBuilder<T, TKey> MetArchiveConditions(Func<T, bool> condition)
    {
        _query = _query.Where(x => condition(x));
        return this;
    }
}
