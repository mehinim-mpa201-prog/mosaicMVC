namespace MosaicMVC.Areas.Admin.ViewModels;

public class TeamUpdateVM
{
    public string Name { get; set; } = null!;
    public string About { get; set; } = null!;
    public IFormFile? Img { get; set; } 
    public int PositionId { get; set; }
}
