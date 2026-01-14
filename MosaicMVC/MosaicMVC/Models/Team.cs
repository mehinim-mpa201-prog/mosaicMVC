using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace MosaicMVC.Models;

public class Team:BaseEntity
{
    public string Name { get; set; } = null!;
    public string About { get; set; } = null!;
    public string ImagePath { get; set; } = null!;

    public int PositionId { get; set; }
    public Position Position { get; set; } = null!;
}


public class Position:BaseEntity
{
    public string Name { get; set; } = null!;
    public List<Team> Teams { get; set; } = new();
}