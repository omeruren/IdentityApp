using AspNetCoreIdentityApp.Core.Models;
using AspNetCoreIdentityApp.Core.ViewModels;
using AspNetCoreIdentityApp.Repository.Models;
using AspNetCoreIdentityApp.Service.Services;
using AspNetCoreIdentityApp.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace AspNetCoreIdentityApp.Web.Controllers;

[Authorize]
public class MemberController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IFileProvider _fileProvider;
    private readonly IMemberService _memberService;
    private readonly TwoFactorialService _twoFactorialService;
    private string userName => User.Identity!.Name!;
    public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider, IMemberService memberService, TwoFactorialService twoFactorialService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _fileProvider = fileProvider;
        _memberService = memberService;
        _twoFactorialService = twoFactorialService;
    }
    public async Task<IActionResult> Index()
    {

        return View(await _memberService.GetUserViewModelByUserNameAsync(userName));
    }
    public async Task Logout()
    {
        await _memberService.LogoutAsync();

    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel request)
    {

        if (!ModelState.IsValid)
        {
            return View(request);

        }

        if (!await _memberService.CheckPasswordAsync(userName, request.OldPassword))
        {
            ModelState.AddModelError(string.Empty, "Old Password is incorrect");
            return View();
        }

        var (isSuccess, errors) = await _memberService.ChangePasswordAsync(userName, request.OldPassword, request.NewPassword);


        if (!isSuccess)
        {
            ModelState.AddModelErrorList(errors!);
            return View();
        }


        TempData["SuccessMessage"] = "Your password has been changed successfully.";

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> UserEdit()
    {

        ViewBag.Gender = _memberService.GetGenderSelectList();

        return View(await _memberService.GetUserEditViewModelAsync(userName));
    }

    [HttpPost]
    public async Task<IActionResult> UserEdit(UserEditViewModel request)
    {
        if (!ModelState.IsValid)
            return View();

        var (isSuccess, errors) = await _memberService.EditUserAsync(request, userName);

        if (!isSuccess)
        {
            ModelState.AddModelErrorList(errors);
            return View();
        }

        TempData["SuccessMessage"] = "User Credentials updated successfully";

        return View(await _memberService.GetUserEditViewModelAsync(userName));
    }

    public IActionResult AccessDenied(string returnUrl)
    {
        string message = string.Empty;
        message = "You do not have been authorized yet. Please contact to Admins";
        ViewBag.message = message;
        return View();
    }

    [HttpGet]
    public IActionResult Claims()
    {

        return View(_memberService.GetClaims(User));
    }

    [Authorize(Policy = "GaziantepPolicy")]
    [HttpGet]
    public IActionResult GaziantepPage()
    {
        return View();
    }
    [Authorize(Policy = "ExchangeExpireDate")]
    [HttpGet]
    public IActionResult ExchangePolicy()
    {
        return View();
    }
    [Authorize(Policy = "ViolencePolicy")]
    [HttpGet]
    public IActionResult ViolencePage()
    {
        return View();
    }

    public async Task<IActionResult> TwoFactorWithAuthenticatior()
    {
        var user = await _userManager.FindByNameAsync(userName);
        string unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);

        if (string.IsNullOrEmpty(unformattedKey))
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
            unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
        }
        AuthenticatiorViewModel authenticatiorViewModel = new AuthenticatiorViewModel();
        authenticatiorViewModel.SharedKey = unformattedKey;
        authenticatiorViewModel.AuthenticationUri = _twoFactorialService.GenerateQrCodeUri(user.Email, unformattedKey);

        return View(authenticatiorViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> TwoFactorWithAuthenticatior(AuthenticatiorViewModel authenticatiorViewModel)
    {
        return View();
    }
    public async Task<IActionResult> TwoFactor()
    {
        var user = await _userManager.FindByNameAsync(userName);
        return View(new AuthenticatiorViewModel() { TwoFactorType = (TwoFactorialAuth)user.TwoFactor });
    }

    [HttpPost]
    public async Task<IActionResult> TwoFactor(AuthenticatiorViewModel request)
    {
        var user = await _userManager.FindByNameAsync(userName);
        switch (request.TwoFactorType)
        {
            case TwoFactorialAuth.None:
                user.TwoFactorEnabled = false;
                user.TwoFactor = (sbyte)TwoFactorialAuth.None;
                TempData["SuccessMessage"] = "Two Factor Authentication disabled successfully";
                break;
            case TwoFactorialAuth.MicrosoftGoogle:
                return RedirectToAction("TwoFactorWithAuthenticatior");
        }
        await _userManager.UpdateAsync(user);
        return View(request);
    }
}
