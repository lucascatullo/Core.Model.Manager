

namespace Code.Models.Manager.Model;

public class QueryPage<T>
{
    public IEnumerable<T> Items { get; set; }
    public bool HasNextPage { get; set; }
}
