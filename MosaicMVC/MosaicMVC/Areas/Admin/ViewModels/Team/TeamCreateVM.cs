namespace MosaicMVC.Areas.Admin.ViewModels;

public class TeamCreateVM
{
    public string Name { get; set; } = null!;
    public string About { get; set; } = null!;
    public IFormFile Img { get; set; } = null!;
    public int PositionId { get; set; }
}
