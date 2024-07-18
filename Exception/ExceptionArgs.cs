namespace Core.Models.Manager.Exception;

[Serializable]
public abstract class ExceptionArgs
{
    /// <summary>
    /// Descriptive message for understanding the exception. 
    /// </summary>
    public virtual string Message
    {
        get { return "An exception has ben thrown: "; }
    }
    /// <summary>
    /// Auto generated ID. Usefull for logging. 
    /// </summary>
    public Guid Id = Guid.NewGuid();
    /// <summary>
    /// Descriptive string code. Ex: EXTERNAL_API_RESPONSE_WAS_500
    /// </summary>
    public virtual string DescriptiveStringCode { get => "UNDEFINED"; }
    /// <summary>
    /// Http error code
    /// </summary>
    public virtual int ErrorCode { get => 0; }
    /// <summary>
    /// Used for models with many validations errors.
    /// </summary>
    public virtual ICollection<string>? Errors { get => null; }

    /// <summary>
    /// This error should be user friendly. Front ent can use this to show an unexpected error. 
    /// </summary>
    public virtual string FancyError { get { return "There was a problem. Please, contact with the site administrator"; } }
}