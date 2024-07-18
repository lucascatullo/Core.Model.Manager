namespace Core.Models.Manager.Interface;

public interface IDeleteDTO
{
    public DateTime Time { get; set; }
    public bool Deleted { get; set; }
}