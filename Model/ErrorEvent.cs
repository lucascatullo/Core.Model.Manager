namespace Core.Models.Manager.Model;

public class ErrorEvent : IErrorEvent
{
    public string Message { get; set; }
}