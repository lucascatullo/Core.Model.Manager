namespace Core.Models.Manager.Model;

public interface IIdentifier<TKey>
{
    public TKey Id { get; set; }
}