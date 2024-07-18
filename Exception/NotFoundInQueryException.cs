namespace Core.Models.Manager.Exception;

/// <summary>
/// Exception Arg initializer
/// </summary>
/// <param name="table">the table in wich was run the query </param>
/// <param name="searchField">search param (Id , Name etc...)</param>
/// <param name="value">Searched value</param>
public class NotFoundInQueryException(string table, string searchField, string value) : ExceptionArgs
{
    private readonly string table = table;
    private readonly string searchField = searchField;
    private readonly string value = value;

    public override int ErrorCode => 404;
    public override string Message => string.Format("the value {2} could not be found in the query executed over the table {0} with search field param {1}", table, searchField, value);
    public override string DescriptiveStringCode => "QUERY_HAS_NO_RESULTS";
}