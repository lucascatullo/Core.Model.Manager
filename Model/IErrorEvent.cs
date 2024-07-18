namespace Core.Models.Manager.Model;

public interface IErrorEvent
{
    string Message { get; set; }
}