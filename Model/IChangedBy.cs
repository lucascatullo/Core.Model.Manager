namespace Core.Models.Manager.Model;

/// <summary>
/// implement this interface on object to know for who the object was created and a log of edits.
/// </summary>
public interface IChangedBy<T> where T : IEditModel
{
    /// <summary>
    /// Current user ID 
    /// </summary>
    public string CreatedById { get; set; }
    /// <summary>
    /// Edit by model.
    /// </summary>
    public IList<T> EditBy { get; set; }
}