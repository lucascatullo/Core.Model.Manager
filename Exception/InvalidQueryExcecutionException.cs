namespace Core.Models.Manager.Exception;

public class InvalidQueryExcecutionException : ExceptionArgs
{
    public override string Message => base.Message + "QueryBuilder objects can't be executed more than once.";
    public override int ErrorCode => 500;
    public override string DescriptiveStringCode => "MULTIPLE_EXECUTIONS_EXCEPTION";
}