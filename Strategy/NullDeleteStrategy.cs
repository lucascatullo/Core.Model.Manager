using Core.Models.Manager.Interface;

namespace Core.Models.Manager.Strategy;

/// <summary>
/// Null delete strategy. Always returns true.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="Guid"></typeparam>
public class NullDeleteStrategy<T, TKey> : IDeleteStrategy<T, TKey> where T : notnull, IBaseDbModel<TKey> where TKey : notnull
{
    public bool CanBeDeleted(T tobj, string? userId = null, string? userRoles = null) => true;
}
