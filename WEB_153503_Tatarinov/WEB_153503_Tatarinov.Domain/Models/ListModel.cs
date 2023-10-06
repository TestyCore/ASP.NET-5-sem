namespace WEB_153503_Tatarinov.Domain.Models;

public class ListModel<T>
    {
    // queried items
    public List<T> Items { get; set; } = new();
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
}
