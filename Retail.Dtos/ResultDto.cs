using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Retail.DTOs;

/// <summary>
/// Represents a Generic Result Dto
/// </summary>
public class ResultDto<T>
{
    /// <summary>
    /// Gets or sets the Is Success
    /// </summary> 
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the Dynamic Model
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Gets or sets the Error Code
    /// </summary> 
    public string? ErrorCode { get; set; }

    /// <summary>
    /// Gets or sets the Error Message
    /// </summary> 
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the Status
    /// </summary> 
    [JsonIgnore]
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

};

