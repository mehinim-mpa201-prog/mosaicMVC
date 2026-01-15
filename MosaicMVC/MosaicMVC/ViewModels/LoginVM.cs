using System.ComponentModel.DataAnnotations;

namespace MosaicMVC.ViewModels;

public class LoginVM
{
    [Required, EmailAddress]
    public string EmailAddress { get; set; } = string.Empty;

    [Required, MaxLength(256), MinLength(6), DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}