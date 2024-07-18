using Core.Models.Manager.Constant;

namespace Core.Models.Manager.Interface;

public interface IFilter<TKey>
{
    DateTime? CreatedDateFrom { get; set; }
    DateTime? CreatedDateTo { get; set; }
    DateTime? ModifiedDateFrom { get; set; }
    DateTime? ModifiedDateTo { get; set; }
    IEnumerable<TKey> Ids { get; set; }
    IEnumerable<TKey> Exclude { get; set; }
    OrderByDate? OrderByDate { get; set; }
}