namespace Core.Models.Manager.Interface;

public interface IMediaModel<TMedia, TKey> where TMedia : IMedia, new()
{
    public TKey MediaId { get; set; }
    public TMedia Media { get; set; }
}