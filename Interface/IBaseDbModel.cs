using System.ComponentModel.DataAnnotations;

namespace Core.Models.Manager.Interface;

public interface IBaseDbModel<Tkey> where Tkey : notnull
{
    [Key]
    public Tkey Id { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public bool LogicalDelete { get; set; }
}