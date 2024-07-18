using Core.Models.Manager.Constant;
using Core.Models.Manager.DTO;
using Core.Models.Manager.Exception;
using Core.Models.Manager.Interface;
using Core.Models.Manager.Model;
using System.Text;

namespace Core.Models.Manager.DataManager;

/// <summary>
/// Base clase for datamanager, Datamanagers has the responsability to establish the C.R.U.D operations 
/// and business logic of data base entities.
/// DATA MANAGER DO NOT WRITE OR EXECUTE QUERYS.
/// </summary>
public class BaseDataManager<TObj, TKey> : IBaseDataManager<TObj, TKey> where TObj : IBaseDbModel<TKey>
{
    public TObj dataBaseObj;
    protected StringBuilder changesBuilder;
    public BaseDataManager(TObj dataBaseObj)
    {
        this.dataBaseObj = dataBaseObj;
        changesBuilder = new StringBuilder("");
        changesBuilder.Append(string.Format("Objeto: {0} Cmabios: ", dataBaseObj.GetType().Name));
    }
    public BaseDataManager()
    {
        changesBuilder = new StringBuilder("");
        changesBuilder.Append(string.Format("Objeto: {0} Cmabios: ", typeof(TObj).Name));
    }

    public void SetCreatedDate() => dataBaseObj.CreatedDate = DateTime.UtcNow;

    public void SetLastUpdateDate() => dataBaseObj.ModifiedDate = DateTime.UtcNow;


    /// <summary>
    /// Delete from memory logic the current object. 
    /// </summary>
    /// <param name="deleteConditions">Pass a class that has to implement IDeleteStrategy.
    /// If you don't want to make any especial delete condition pass NullDeleteStrategy as param.</param>
    /// <returns></returns>
    public virtual bool Delete(IDeleteStrategy<TObj, TKey> deleteConditions)
    {
        if (deleteConditions.CanBeDeleted(dataBaseObj))
        {
            dataBaseObj.LogicalDelete = true;
            SetLastUpdateDate();
            return true;
        }
        return false;
    }

    public IDeleteDTO GetDeleteInfoDTO()
    {
        var response = new DeleteDTO();
        response.Time = dataBaseObj.ModifiedDate;
        response.Deleted = dataBaseObj.LogicalDelete;
        return response;
    }

    public virtual void SetValues(From from = From.CREATE)
    {
        if (from == From.CREATE)
            SetCreatedDate();
        SetLastUpdateDate();
    }
    /// <summary>
    /// Compares the input object with the data base object and finds all fields with same name/type with different values.
    /// </summary>
    /// <param name="changes"></param>
    public virtual void CompareWith(object changes)
    {
        changesBuilder.Append("Object Id: " + dataBaseObj.Id?.ToString());
        var dataBaseObjType = dataBaseObj.GetType();
        var changesType = changes.GetType();
        var commonProperties = from s in dataBaseObjType.GetProperties().ToList()
                               from t in changesType.GetProperties().ToList()
                               where s.Name == t.Name &&
                                     s.CanRead &&
                                     t.CanWrite &&
                                     s.PropertyType == t.PropertyType
                               select new ModelPropertyMap
                               {
                                   Source = s,
                                   Target = t
                               };
        foreach (var property in commonProperties)
        {
            if (property.Source.GetValue(dataBaseObj, null) != property.Target.GetValue(changes, null))
            {
                var changeTextSource = property.Source.GetValue(dataBaseObj, null) == null ? "Vacio" :
                    property.Source.GetValue(dataBaseObj, null).ToString();
                var changeTextTarget =
                    property.Target.GetValue(changes, null) == null ? "Vacio" : property.Target.GetValue(changes, null).ToString();
                changesBuilder.Append(string.Format("La propiedad {0} ha cambiado de valor de {1} a {2}",
                    property.Source.Name, changeTextSource, changeTextTarget));
            }
        }
    }

    /// <summary>
    /// Adds an historic message. Used this when a change occurs on the db model and you want to keep a record of that.
    /// </summary>
    /// <param name="message">Message to be saved.</param>
    /// <param name="userId">User who did the change</param>
    /// <exception cref="ArgumentException">If the model doesn't implement the IHistory interface.</exception>
    public void SetHistorical(string message, string userId)
    {
        var history = dataBaseObj as IHistory;
        if (history == null) throw new ArgumentException("To use this method databaseobj should implement the interface IHistory");
        if (history.History == null)
            throw new Exception<NullException>(new NullException("History"));
        history.History.Add(new HistoricalModel
        {
            Message = message,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
            EditedById = userId,
        });
    }

    protected string GetLogOfChanges() => changesBuilder.ToString();

}