namespace Library.Server.BL.Models;

public record Condition
{
    public int Id { get; set; }
    public required string Name { get; set; }
}