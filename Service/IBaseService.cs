
namespace Code.Models.Manager.Service;

public interface IBaseService<T,TKey> where TKey : notnull
{
    Task<bool> LogicalDeleteAsync(TKey id);
    Task<bool> LogicalDeleteAsync(TKey id, string? userId, string? userRoles);
    Task<T> GetAsync(TKey id);
}