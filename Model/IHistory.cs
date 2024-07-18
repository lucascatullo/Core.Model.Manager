namespace Core.Models.Manager.Model;

public interface IHistory
{
    public IList<HistoricalModel> History { get; set; }
}