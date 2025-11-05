using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFileProvider _fileProvider;
        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

            var userViewModel = new UserViewModel { Email = currentUser!.Email, Phone = currentUser.PhoneNumber, UserName = currentUser.UserName, PictureUrl = currentUser.Picture };

            return View(userViewModel);
        }
        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();

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

            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser, request.OldPassword);

            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Old Password is incorrect");
                return View();
            }
            var result = await _userManager.ChangePasswordAsync(currentUser, request.OldPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors);
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, request.NewPassword, true, false);

            TempData["SuccessMessage"] = "Your password has been changed successfully.";

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UserEdit()
        {
            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName!,
                Email = currentUser.Email!,
                Phone = currentUser.PhoneNumber!,
                BirthDate = currentUser.BirthDate,
                City = currentUser.City,
                Gender = currentUser.Gender,
            };
            return View(userEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel request)
        {
            if (!ModelState.IsValid)
                return View();

            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

            currentUser.UserName = request.UserName;
            currentUser.Email = request.Email;
            currentUser.PhoneNumber = request.Phone;
            currentUser.BirthDate = request.BirthDate;
            currentUser.City = request.City;
            currentUser.Gender = request.Gender;


            if (request.Picture != null && request.Picture.Length > 0)
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");

                var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.Picture.FileName)}";

                var newPicturePath = Path.Combine(wwwrootFolder.First(x => x.Name == "userPictures").PhysicalPath!, randomFileName);

                using var stream = new FileStream(newPicturePath, FileMode.Create);
                await request.Picture.CopyToAsync(stream);

                currentUser.Picture = randomFileName;
            }
            var updatedUser = await _userManager.UpdateAsync(currentUser);

            if (!updatedUser.Succeeded)
            {
                ModelState.AddModelErrorList(updatedUser.Errors);
                return View();
            }
            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();

            if (request.BirthDate.HasValue)
                await _signInManager.SignInWithClaimsAsync(currentUser, true, new[] { new Claim("birthdate", currentUser!.BirthDate!.Value.ToString()) });
            else
                await _signInManager.SignInAsync(currentUser, true);



            TempData["SuccessMessage"] = "User Credentials updated successfully";

            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName!,
                Email = currentUser.Email!,
                Phone = currentUser.PhoneNumber!,
                BirthDate = currentUser.BirthDate,
                City = currentUser.City,
                Gender = currentUser.Gender,
            };
            return View(userEditViewModel);
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
            var userClaims = User.Claims.Select(x => new ClaimViewModel()
            {
                Issuer = x.Issuer,
                Type = x.Type,
                Value = x.Value,
            }).ToList();
            return View(userClaims);
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

    }
}
