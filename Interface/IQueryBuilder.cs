using Core.Models.Manager.Constant;
using Core.Models.Manager.DTO;

namespace Core.Models.Manager.Interface;

public interface IQueryBuilder<T, TKey> where T : notnull, IBaseDbModel<TKey> where TKey : notnull
{
    bool AllowMultipleExecution { get; set; }

    T Get(TKey id);
    T? GetOrDefault(TKey id);
    Task<T> GetAsync(TKey id);
    Task<IList<T>> GetAllAsync(IList<TKey> ids);
    Task<T?> GetAsyncOrDefault(TKey id);
    Task<T?> FirstOrDefaultAsync();
    IQueryable<T> GetQuery();
    IList<T> Execute();
    Task<IList<T>> ExecuteAsync();
    Task<IList<T>> ExecuteAsync(int number);
    IQueryBuilder<T, TKey> FromDate(DateTime from);
    IQueryBuilder<T, TKey> ToDate(DateTime to);
    IQueryBuilder<T, TKey> TimePediod(DateTime from, DateTime to);
    IQueryBuilder<T, TKey> Exclude(TKey id);
    IQueryBuilder<T, TKey> ExcludeRange(IEnumerable<TKey> ids);
    int? GetTotalPages();
    Task<IList<T>> PaginateAsync(int page = 1, int pageSize = 5);
    bool PaginationHasNextPage();
    int GetTotalResults();
    Task<TotalPagesAndResultsVM> GetTotalPagesAndResultsAsync();
    IQueryBuilder<T, TKey> Filter(IFilter<TKey> filter);
    IQueryBuilder<T, TKey> OrderByDate(OrderByDate condition);
}