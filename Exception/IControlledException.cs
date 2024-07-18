namespace Core.Models.Manager.Exception;

public interface IControlledException
{
    int ErrorCode { get; }
    ICollection<string>? Errors { get; }
    string FancyError { get; }
    string Message { get; }
    string DescriptiveCode { get; }
}