using Core.Models.Manager.Interface;

namespace Core.Models.Manager.DataManager;

public class MediaManager<T, TMedia, TKey> : BaseDataManager<T, TKey> where T : IMediaModel<TMedia, TKey>, IBaseDbModel<TKey> where TMedia : IMedia, new()
{
    public void SetMedia(IMedia media)
    {
        dataBaseObj.Media = new()
        {
            ContentType = media.ContentType,
            Path = media.Path,
            PathLowResolution = media.PathLowResolution,
            PathMediumResolution = media.PathMediumResolution,
            Name = media.Name,
        };
    }

    public void SetMedia(IMedia media, Action<IMedia> onCreate)
    {
        dataBaseObj.Media = new()
        {
            ContentType = media.ContentType,
            Path = media.Path,
            PathLowResolution = media.PathLowResolution,
            PathMediumResolution = media.PathMediumResolution,
            Name = media.Name
        };
        onCreate.Invoke(media);
    }
}