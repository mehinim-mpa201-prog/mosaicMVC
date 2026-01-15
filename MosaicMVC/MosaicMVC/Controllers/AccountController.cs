using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MosaicMVC.Models;
using MosaicMVC.ViewModels;

namespace MosaicMVC.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }


    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        if (!ModelState.IsValid)
        {
            return View(registerVM);
        }

        AppUser appUser = new AppUser
        {
            FirstName = registerVM.FirstName,
            LastName = registerVM.LastName,
            Email = registerVM.EmailAddress,
            UserName = registerVM.UserName,
        };

        var result = await _userManager.CreateAsync(appUser, registerVM.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
                return View(registerVM);
            }
        }

        result = await _userManager.AddToRoleAsync(appUser, "Member");

     

        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        if (!ModelState.IsValid)
        {
            return View(loginVM);
        }

        var appUser = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
        if (appUser == null)
        {
            ModelState.AddModelError("", "Invalid Email or Password");
            return View(loginVM);
        }

        var isCorrectPassword = await _userManager.CheckPasswordAsync(appUser, loginVM.Password);
        if (!isCorrectPassword)
        {
            ModelState.AddModelError("", "Invalid Email or Password");
            return View(loginVM);
        }
       

        await _signInManager.SignInAsync(appUser, loginVM.RememberMe);

        return RedirectToAction(nameof(Index), "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Index), "Home");
    }

    public async Task<IActionResult> CreateAdminAndModerator()
    {
        var adminUserVM = _configuration.GetSection("AdminUser").Get<UserVM>();

        if (adminUserVM is { })
        {
            AppUser admin = new AppUser
            {
                FirstName = adminUserVM.FirstName,
                LastName = adminUserVM.LastName,
                UserName = adminUserVM.UserName,
                Email = adminUserVM.Email,

            };
            var result = await _userManager.CreateAsync(admin, adminUserVM.Password);
            result = await _userManager.AddToRoleAsync(admin, "Admin");

        }

        var moderatorUserVM = _configuration.GetSection("ModeratorUser").Get<UserVM>();

        if (moderatorUserVM is { })
        {
            AppUser moderator = new AppUser
            {
                FirstName = moderatorUserVM.FirstName,
                LastName = moderatorUserVM.LastName,
                UserName = moderatorUserVM.UserName,
                Email = moderatorUserVM.Email,

            };
            var result = await _userManager.CreateAsync(moderator, moderatorUserVM.Password);
            result = await _userManager.AddToRoleAsync(moderator, "Moderator");

        }

        return RedirectToAction(nameof(Index), "Home");
    }

    public async Task<IActionResult> CreateRoles()
    {
        await _roleManager.CreateAsync(new IdentityRole("Admin"));
        await _roleManager.CreateAsync(new IdentityRole("Moderator"));
        await _roleManager.CreateAsync(new IdentityRole("Member"));
        return RedirectToAction(nameof(Index), "Home");
    }

}
