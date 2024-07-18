namespace Core.Models.Manager.Interface;

/// <summary>
/// This class has responsability for determine if a data base entry can or not be deleted.
/// </summary>
/// <typeparam name="T">Type of database model</typeparam>
/// <typeparam name="TKey">Type of data base model key</typeparam>
public interface IDeleteStrategy<T, TKey> where T : IBaseDbModel<TKey>
{
    public bool CanBeDeleted(T tobj);
}