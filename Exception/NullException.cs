
namespace Core.Models.Manager.Exception;

public class NullException : ExceptionArgs
{
    private readonly string nullObjectName;
    public NullException(string nullObjectName)
    {
        this.nullObjectName = nullObjectName;
    }
    public static void TrhowIfNull(object obj)
    {
        if (obj is null)
            throw new Exception<NullException>(new NullException(nameof(obj)));
    }
    public override int ErrorCode => 500;
    public override string FancyError => base.FancyError;
    public override string DescriptiveStringCode => "NULL_OBJECT_REFERENCE";
    public override string Message => base.Message + string.Format("Null object exception. {0} was null (are you missing an include?)", nullObjectName);
}