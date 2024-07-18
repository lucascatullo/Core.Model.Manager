using Core.Models.Manager.Interface;

namespace Core.Models.Manager.Model;

public class Media : IMedia
{
    public string PathLowResolution { get; set; }
    public string PathMediumResolution { get; set; }
    public string Path { get; set; }
    public string ContentType { get; set; }
    public string Name { get; set; }
}