

namespace Code.Models.Manager.Model;

public interface IArchiveModel
{
    public DateTime? ArchivedDate { get; set; }

    public bool IsArchived { get; set; } 
}
