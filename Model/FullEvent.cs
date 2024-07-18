namespace Core.Models.Manager.Model;

public class FullEvent<T> : ISucessEvent<T>, IErrorEvent
{
    public string Message { get; set; }
    public T Obj { get; set; }
    public bool IsSuccessEvent { get; set; }
}