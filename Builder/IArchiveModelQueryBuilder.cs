using Code.Models.Manager.Model;
using Core.Models.Manager.Interface;
using Core.Models.Manager.Model;

namespace Code.Models.Manager.Builder;

public interface IArchiveModelQueryBuilder<T, TKey> : IQueryBuilder<T, TKey>
    where T : notnull, BaseModel<TKey>, IArchiveModel
    where TKey : notnull
{
    IArchiveModelQueryBuilder<T, TKey> IgnoreArchived();

    IArchiveModelQueryBuilder<T, TKey> MetArchiveConditions(int days);
    IArchiveModelQueryBuilder<T, TKey> MetArchiveConditions(Func<T, bool> condition);
}