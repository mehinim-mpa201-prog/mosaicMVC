namespace MosaicMVC.Models;

public class Position:BaseEntity
{
    public string Name { get; set; } = null!;
    public List<Team> Teams { get; set; } = new();
}