using Core.Models.Manager.Interface;

namespace Core.Models.Manager.DataManager;

public class MultiImageManager<T, TKey, TMedia> : BaseDataManager<T, TKey> where TMedia : IMedia, new() where T : notnull, IBaseDbModel<TKey>, IMultiMediaModel<TMedia> where TKey : notnull
{

    public void SetMedias(IEnumerable<IMedia> medias, Action<TMedia> onCreate)
    {
        dataBaseObj.Medias = [];
        foreach (var media in medias)
            AddMedia(media, onCreate);
    }

    public void SetMedias(IEnumerable<IMedia> medias)
    {
        dataBaseObj.Medias = [];
        foreach (var media in medias)
            AddMedia(media);
    }

    private TMedia CreateMedia(IMedia media) => new()
    {
        ContentType = media.ContentType,
        Path = media.Path,
        PathLowResolution = media.PathLowResolution,
        PathMediumResolution = media.PathMediumResolution,
        Name = media.Name,
    };

    public void AddMedia(IMedia[] medias)
    {
        foreach (var media in medias)
            AddMedia(media);
    }
    public void AddMedia(IMedia media)
    {
        if (dataBaseObj.Medias is null)
            throw new InvalidOperationException("dataBaseObj.Medias can't be null.");

        dataBaseObj.Medias.Add(CreateMedia(media));
    }
    public void AddMedia(IMedia media, Action<TMedia> onCreate)
    {
        if (dataBaseObj.Medias is null)
            throw new InvalidOperationException("dataBaseObj.Medias can't be null.");
        TMedia temporalMedia = CreateMedia(media);
        onCreate.Invoke(temporalMedia);
        dataBaseObj.Medias.Add(temporalMedia);
    }

    public void RemoveMedia(Func<TMedia, bool> condition) => dataBaseObj.Medias = dataBaseObj.Medias.Where(m => !condition.Invoke(m)).ToList();
}