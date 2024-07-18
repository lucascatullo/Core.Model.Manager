namespace Core.Models.Manager.Model;

public class SucessEvent<TResponse> : ISucessEvent<TResponse>
{
    public TResponse Obj { get; set; }
}