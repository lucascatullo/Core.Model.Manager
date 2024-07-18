namespace Core.Models.Manager.Interface;

public interface IMedia
{
    string PathLowResolution { get; set; }
    string PathMediumResolution { get; set; }
    string Path { get; set; }
    string ContentType { get; set; }
    string Name { get; set; }
}