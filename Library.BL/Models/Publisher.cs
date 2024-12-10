namespace Library.BL.Models;

public record Publisher
{
    public int Id { get; set; }
    public required string Name { get; set; }
}