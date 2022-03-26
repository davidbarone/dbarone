namespace dbarone_api.Lib.Validation;

/// <summary>
/// Thrown when there is a validation error.
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// List of validation results.
    /// </summary>
    public IEnumerable<ValidationResult> Results { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="results"></param>
    public ValidationException(IEnumerable<ValidationResult> results) : base("Object has failed validation. Please refer to validation results for more information")
    {
        this.Results = results;
    }
}