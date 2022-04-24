namespace dbarone_api.Models.Response;

/// <summary>
/// The message type
/// </summary>
public enum ResponseMessageType
{
    /// <summary>
    /// Used for validations, errors. The request will not succeed in this case.
    /// </summary>
    ERROR,
    
    /// <summary>
    /// Warnings - the request will still complete in this case.
    /// </summary>
    WARNING,
    
    /// <summary>
    /// Information - the request will still complete in this case.
    /// </summary>
    INFORMATION
}