namespace WEB_153503_Tatarinov.Domain.Models;

public class ResponseData<T>
{
    // queried data
    public T Data { get; set; }
    public bool Success { get; set; } = true;
    public string? ErrorMessage { get; set; }
}