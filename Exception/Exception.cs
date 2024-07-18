using System.Runtime.Serialization;

namespace Core.Models.Manager.Exception;

[Serializable]
public sealed class Exception<TExceptionArgs> : System.Exception, ISerializable, IControlledException where TExceptionArgs : ExceptionArgs
{
    private readonly TExceptionArgs m_args;

    public TExceptionArgs Args
    {
        get
        {
            return m_args;
        }
    }


    public Exception(string? message = null, System.Exception? innerException = null) : this(null, message, innerException) { }

    public Exception(TExceptionArgs args, string? message = null, System.Exception? innerException = null) : base(message, innerException)
    {
        m_args = args;
    }

    public override string Message
    {
        get
        {
            return m_args.Message ?? m_args.Message.ToString();
        }
    }

    public ICollection<string>? Errors { get => m_args.Errors; }

    public int ErrorCode { get => m_args.ErrorCode; }

    public string FancyError { get => m_args.FancyError; }
    public string DescriptiveCode { get => m_args.DescriptiveStringCode; }

    public override bool Equals(object obj)
    {
        Exception<TExceptionArgs>? other = obj as Exception<TExceptionArgs>;

        return other != null && Equals(m_args, other.m_args) && base.Equals(obj);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}