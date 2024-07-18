using Core.Models.Manager.Interface;

namespace Core.Models.Manager.Model;

public class BaseModel<Tkey> : IBaseDbModel<Tkey>, IIdentifier<Tkey> where Tkey : notnull
{
    public Tkey Id { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime ModifiedDate { get; set; } = DateTime.Now;
    public bool LogicalDelete { get; set; }
}