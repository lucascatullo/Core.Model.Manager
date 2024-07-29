using Core.Models.Manager.Constant;
using Core.Models.Manager.DTO;
using Core.Models.Manager.Exception;
using Core.Models.Manager.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Core.Models.Manager.Builder;

/// <summary>
/// T: Object type (Data base Model)
/// Tkey: data type of the object id in the data base
/// </summary>
public class QueryBuilder<T, TKey> : IQueryBuilder<T, TKey>, IDisposable where T : notnull, IBaseDbModel<TKey> where TKey : notnull
{
    public IQueryable<T> _query;

    private bool hasBeenPaginated = false;
    private int? pageSize;
    private int? pageNum;
    private bool _disposed = false;
    private bool _executed = false;

    /// <summary>
    /// Set this to true when need to execute a query multiple times. 
    /// This is always recommended as false.
    /// </summary>
    public bool AllowMultipleExecution { get; set; } = false;

    private readonly SafeHandle _safeHandle = new SafeFileHandle(nint.Zero, true);
    private void CheckExcecutionStatus()
    {
        if (_executed && !AllowMultipleExecution)
            throw new Exception<InvalidQueryExcecutionException>(new InvalidQueryExcecutionException());
    }

    public IList<T> paginatedObj;

    /// <summary>
    /// Initialize the QueryBuilder class.
    /// </summary>
    /// <param name="initialQuery">objet IQueryable from DB Ej ApplicationDBContext.AspNetUser</param>
    public QueryBuilder(IQueryable<T> initialQuery) =>
        _query = initialQuery.Where(x => !x.LogicalDelete);


    /// <summary>
    /// Executes the query and return the result.
    /// </summary>
    /// <returns></returns>
    public IList<T> Execute()
    {
        CheckExcecutionStatus();
        _executed = true;
        return _query.ToList();
    }
    /// <summary>
    /// Executes the query and return the result in an async way. Use this to avoid multiple threading exceptions.
    /// </summary>
    /// <returns></returns>
    public async Task<IList<T>> ExecuteAsync()
    {
        CheckExcecutionStatus();
        _executed = true;
        return await _query.ToListAsync();
    }

    /// <summary>
    /// Executes the query async but only return the quantity results
    /// </summary>
    /// <param name="quantity">number of results</param>
    /// <returns></returns>
    public async Task<IList<T>> ExecuteAsync(int quantity)
    {
        CheckExcecutionStatus();
        _executed = true;
        return await _query.Take(quantity).ToListAsync();
    }

    /// <summary>
    /// Filter the query base on creation date.
    /// </summary>
    /// <param name="from">Started date</param>
    /// <returns>This</returns>
    public IQueryBuilder<T, TKey> FromDate(DateTime from)
    {
        _query = _query.Where(x => x.CreatedDate >= from);
        return this;
    }
    /// <summary>
    /// Filter the query base on creation date.
    /// </summary>
    /// <param name="from">Started date</param>
    /// <returns>This</returns>
    public IQueryBuilder<T, TKey> FromModifiedDate(DateTime from)
    {
        _query = _query.Where(x => x.ModifiedDate >= from);
        return this;
    }
    /// <summary>
    /// Get the object T with Id 'id' on the current query, if no result are found, it throws an exception.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>T</returns>
    public T Get(TKey id)
    {
        NullException.TrhowIfNull(id);
        CheckExcecutionStatus();
        _executed = true;
        var response = _query.FirstOrDefault(x => x.Id.Equals(id));
        if (response == null) throw new Exception<NotFoundInQueryException>(new NotFoundInQueryException(typeof(T).ToString(), "Id", id.ToString()!));
        return response;
    }

    /// <summary>
    /// Get the object T with Id 'id' on the current query, if no result are found, it throws an exception.
    /// Use this method to avoid multi threading exceptions
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>

    public async Task<T> GetAsync(TKey id)
    {
        NullException.TrhowIfNull(id);

        CheckExcecutionStatus();
        _executed = true;
        var response = await _query.FirstOrDefaultAsync(x => x.Id.Equals(id));
        if (response == null) throw new Exception<NotFoundInQueryException>(new NotFoundInQueryException(typeof(T).ToString(), "Id", id.ToString()!));
        return response;
    }

    public IQueryBuilder<T, TKey> Exclude(TKey id)
    {
        _query = _query.Where(x => !x.Id.Equals(id));
        return this;
    }

    public IQueryBuilder<T, TKey> ExcludeRange(IEnumerable<TKey> ids)
    {
        foreach (var i in ids) Exclude(i);
        return this;
    }



    /// <summary>
    /// Get the object T with Id 'id' on the current query, if no result are found, return null.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public T? GetOrDefault(TKey id)
    {
        CheckExcecutionStatus();
        _executed = true;
        return _query.FirstOrDefault(x => x.Id.ToString()!.Equals(id));
    }

    public async Task<T?> GetAsyncOrDefault(TKey id)
    {
        CheckExcecutionStatus();
        _executed = true;
        return await _query.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    /// <summary>
    /// Get the query on his current state.
    /// </summary>
    /// <returns></returns>
    public IQueryable<T> GetQuery() => _query;

    /// <summary>
    /// Get the objects that were created between values. 
    /// </summary>
    /// <param name="from">Started created date</param>
    /// <param name="to">End created date</param>
    /// <returns></returns>
    public IQueryBuilder<T, TKey> TimePediod(DateTime from, DateTime to)
    {
        _query = _query.Where(x => x.CreatedDate >= from && x.CreatedDate <= to);
        return this;

    }

    /// <summary>
    /// Filter the query with the object that were created before a Date
    /// </summary>
    /// <param name="to">Date in wich object were created.</param>
    /// <returns></returns>
    public IQueryBuilder<T, TKey> ToDate(DateTime to)
    {
        _query = _query.Where(x => x.CreatedDate <= to);
        return this;

    }
    /// <summary>
    /// Filter the query with the object that were created before a Modified Date
    /// </summary>
    /// <param name="to">Date in wich object were created.</param>
    /// <returns></returns>
    public IQueryBuilder<T, TKey> ToModifiedDate(DateTime to)
    {
        _query = _query.Where(x => x.ModifiedDate <= to);
        return this;

    }


    /// <summary>
    /// This method executes the query if it wasn't been executed before.
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public IList<T> Paginate(int page = 1, int pageSize = 5)
    {
        CheckExcecutionStatus();
        _executed = true;
        if (!hasBeenPaginated)
        {
            paginatedObj = _query.Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
        }


        this.pageSize = pageSize;
        hasBeenPaginated = true;
        pageNum = page;
        return paginatedObj;
    }

    public async Task<IList<T>> PaginateAsync(int page = 1, int pageSize = 5)
    {
        CheckExcecutionStatus();
        _executed = true;

        if (!hasBeenPaginated)
        {
            paginatedObj = await _query.Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
        }


        this.pageSize = pageSize;
        hasBeenPaginated = true;
        pageNum = page;
        return paginatedObj;
    }

    /// <summary>
    /// Calculatees if the paginated object has next page
    /// </summary>
    /// <returns>true if it has next page or false if doesn't have next page or the object hasn't been paginated.</returns>
    public bool PaginationHasNextPage() => hasBeenPaginated && _query.Count() > pageNum * pageSize;
    public int? GetTotalPages() => pageSize != 0 && pageSize != null ? (int)Math.Ceiling((double)_query.Count() / pageSize.Value) : 0;
    public int GetTotalResults() => _query.Count();

    public async Task<TotalPagesAndResultsVM> GetTotalPagesAndResultsAsync()
    {
        var count = await _query.CountAsync();
        return new TotalPagesAndResultsVM
        {
            TotalPages = pageSize != 0 && pageSize != null ? (int)Math.Ceiling((double)count / pageSize.Value) : 0,
            TotalResults = count
        };
    }

    public T? FirstOrDefault()
    {
        CheckExcecutionStatus();
        _executed = true;

        return _query.FirstOrDefault();
    }
    public async Task<T?> FirstOrDefaultAsync()
    {
        CheckExcecutionStatus();
        _executed = true;

        return await _query.FirstOrDefaultAsync();
    }

    public async Task<IList<T>> GetAllAsync(IList<TKey> ids)
    {
        CheckExcecutionStatus();
        _executed = true;
        return ids != null ? await _query.Where(x => ids.Contains(x.Id)).ToListAsync() : [];
    }
    public IQueryBuilder<T, TKey> OrderByDate(bool asc = true)
    {
        if (asc)
            _query = _query.OrderBy(x => x.CreatedDate);
        else
            _query = _query.OrderByDescending(x => x.CreatedDate);
        return this;
    }
    public IQueryBuilder<T, TKey> OrderByModifiedDate(bool asc = true)
    {
        if (asc)
            _query = _query.OrderBy(x => x.ModifiedDate);
        else
            _query = _query.OrderByDescending(x => x.ModifiedDate);
        return this;
    }

    /// <summary>
    /// Removes from the query the object with the specified ids.
    /// </summary>
    /// <param name="ids">Ids to remove</param>
    /// <returns>An intance of this class.</returns>
    public IQueryBuilder<T, TKey> ExcludeIds(IEnumerable<TKey> ids)
    {
        _query = _query.Where(x => !ids.Contains(x.Id));
        return this;
    }
    /// <summary>
    /// Returns a query with only the ids passed on the param..
    /// </summary>
    /// <param name="ids">Ids</param>
    /// <returns>An intance of this class.</returns>
    public IQueryBuilder<T, TKey> HasId(IEnumerable<TKey> ids)
    {
        _query = _query.Where(x => ids.Contains(x.Id));
        return this;
    }

    public void Dispose() => Dispose(true);

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
            _safeHandle?.Dispose();

        _disposed = true;
    }


    /// <summary>
    /// Filter the objects in query base on the main fields.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>Filtered IQueryable</returns>
    public IQueryBuilder<T, TKey> Filter(IFilter<TKey> filter)
    {
        if (filter != null)
        {
            if (filter.CreatedDateFrom != null) FromDate(filter.CreatedDateFrom.Value);
            if (filter.CreatedDateTo != null) ToDate(filter.CreatedDateTo.Value);
            if (filter.ModifiedDateFrom != null) FromModifiedDate(filter.ModifiedDateFrom.Value);
            if (filter.ModifiedDateTo != null) ToModifiedDate(filter.ModifiedDateTo.Value);
            if (filter.Ids != null) HasId(filter.Ids);
            if (filter.Exclude != null) ExcludeIds(filter.Exclude);
            if (filter.OrderByDate != null) OrderByDate(filter.OrderByDate.Value);

        }
        return this;
    }

    public IQueryBuilder<T, TKey> OrderByDate(OrderByDate condition)
    {
        switch (condition)
        {
            case Constant.OrderByDate.ASC:
                _query = _query.OrderBy(x => x.CreatedDate);
                break;
            case Constant.OrderByDate.DESC:
                _query = _query.OrderByDescending(x => x.CreatedDate);
                break;
        }
        return this;
    }
}
