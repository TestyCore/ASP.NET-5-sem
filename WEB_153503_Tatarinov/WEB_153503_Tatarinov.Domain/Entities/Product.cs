using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_153503_Tatarinov.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    
    public float Price { get; set; }
    public string? ImgPath { get; set; }
    public string ImgMimeType { get; set; }
    
    public Category? Category { get; set; }
}