namespace Core.Models.Manager.Model;

public interface ISucessEvent<TResponse>
{
    TResponse Obj { get; set; }
}