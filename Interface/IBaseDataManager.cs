
namespace Core.Models.Manager.Interface;

public interface IBaseDataManager<TObj, TKey> where TObj : notnull , IBaseDbModel<TKey> where TKey : notnull
{
    bool Delete(IDeleteStrategy<TObj, TKey> deleteConditions);
    IDeleteDTO GetDeleteInfoDTO();
    void SetLastUpdateDate();
    void SetHistorical(string message, string userId);
    void MakeReadyToArchive();
}