namespace Core.Models.Manager.Model;

public class HistoricalModel : BaseModel<int>, IEditModel
{
    public string EditedById { get; set; }
    public string Message { get; set; }
}