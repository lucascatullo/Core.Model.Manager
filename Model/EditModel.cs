namespace Core.Models.Manager.Model;


/// <summary>
/// objects who implements IChangedBy are going to have a list of EditModel object. Each team someone edits the objects we need to save the id and the date. 
/// </summary>
public interface IEditModel
{
    /// <summary>
    /// Current user ID.
    /// </summary>
    public string EditedById { get; set; }
    /// <summary>
    /// Date of modification.
    /// </summary>
    public DateTime ModifiedDate { get; set; }
}