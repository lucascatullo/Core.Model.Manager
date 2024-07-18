using Core.Models.Manager.Constant;

namespace Core.Models.Manager.Interface;

public interface IBaseDataManager<TObj, TKey> where TObj : IBaseDbModel<TKey>
{
    bool Delete(IDeleteStrategy<TObj, TKey> deleteConditions);
    IDeleteDTO GetDeleteInfoDTO();
    void SetCreatedDate();
    void SetLastUpdateDate();
    void SetValues(From from = From.CREATE);
    void SetHistorical(string message, string userId);
}