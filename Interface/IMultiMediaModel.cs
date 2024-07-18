namespace Core.Models.Manager.Interface;

public interface IMultiMediaModel<TMedia> where TMedia : IMedia, new()
{
    public IList<TMedia> Medias { get; set; }
}