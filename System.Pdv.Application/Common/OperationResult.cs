using System.Text.Json.Serialization;

namespace System.Pdv.Application.Common;

public class OperationResult<T>
{
    public T? Result { get; set; }
    public bool ServerOn { get; set; } = true;
    public string? Message { get; set; }

    [JsonIgnore]
    public int StatusCode { get; set; } = 200;
}
