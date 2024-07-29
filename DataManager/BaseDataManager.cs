using Code.Models.Manager.Extension;
using Code.Models.Manager.Model;
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
public class BaseDataManager<TObj, TKey> : IBaseDataManager<TObj, TKey> where TObj : notnull, IBaseDbModel<TKey> where TKey: notnull
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


    public void SetLastUpdateDate() => dataBaseObj.ModifiedDate = DateTime.UtcNow;


    /// <summary>
    /// Delete from memory logic the current object. 
    /// </summary>
    /// <param name="deleteConditions">Pass a class that has to implement IDeleteStrategy.
    /// If you don't want to make any especial delete condition pass NullDeleteStrategy as param.</param>
    /// <returns></returns>
    public virtual bool Delete(IDeleteStrategy<TObj, TKey> deleteConditions)
    {
        if (deleteConditions.CanBeDeleted(dataBaseObj,null, null))
        {
            dataBaseObj.LogicalDelete = true;
            SetLastUpdateDate();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Delete from memory logic the current object. Use this overlord for taking into account current user information
    /// </summary>
    /// <param name="deleteConditions">Pass a class that has to implement IDeleteStrategy.
    /// If you don't want to make any especial delete condition pass NullDeleteStrategy as param.</param>
    /// <returns></returns>
    public virtual bool Delete(IDeleteStrategy<TObj, TKey> deleteConditions, string? userId , string? userRoles)
    {
        if (deleteConditions.CanBeDeleted(dataBaseObj, userId, userRoles))
        {
            dataBaseObj.LogicalDelete = true;
            SetLastUpdateDate();
            return true;
        }
        return false;
    }

    public IDeleteDTO GetDeleteInfoDTO()
    {
        var response = new DeleteDTO
        {
            Time = dataBaseObj.ModifiedDate,
            Deleted = dataBaseObj.LogicalDelete
        };
        return response;
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
        var commonProperties = dataBaseObjType.GetCommonPropertiesMap(changesType);
        foreach (var property in commonProperties)
        {
            if (property.Source.GetValue(dataBaseObj, null) != property.Target.GetValue(changes, null))
            {
                var changeTextSource = property.Source.GetValue(dataBaseObj, null) == null ? "Vacio" :
                    property.Source.GetValue(dataBaseObj, null)!.ToString();
                var changeTextTarget =
                    property.Target.GetValue(changes, null) == null ? "Vacio" : property.Target.GetValue(changes, null)!.ToString();
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
        IHistory history = dataBaseObj as IHistory ??
            throw new ArgumentException("To use this method databaseobj should implement the interface IHistory");
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


    /// <returns>Return all changes logged in the object.</returns>
    protected string GetLogOfChanges() => changesBuilder.ToString();

    /// <summary>
    /// Sets the archive date and the bool is archived to true. This is a step needed to move the Arvchived objects to another DbContext or delete all of them.
    /// </summary>
    /// <exception cref="ArgumentException">If the model doesn't implement IArchiveModel interface</exception>
    public void MakeReadyToArchive()
    {
        IArchiveModel archiveModel = dataBaseObj as IArchiveModel
            ?? throw new ArgumentException("To use this method DatabaseObj Should implement the interface IArchiveModel");

        archiveModel.ArchivedDate = DateTime.Now;
        archiveModel.IsArchived = true;
    }

    /// <summary>
    /// Checks if the databaseobj is ready to be archived.
    /// </summary>
    /// <param name="days">number of days</param>
    /// <returns> Returns true if the model is older than the quantity of days.</returns>
    public bool NeedsToBeArchived(Func<TObj, bool> condition) => condition(dataBaseObj);
}