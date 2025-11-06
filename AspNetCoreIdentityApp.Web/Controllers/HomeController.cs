using System.Diagnostics;
using System.Threading.Tasks;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreIdentityApp.Web.Extensions;
using System.Formats.Tar;
using AspNetCoreIdentityApp.Web.Services;
using System.Security.Claims;
namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;

        private readonly IEmailService _emailService;
        public HomeController(UserManager<AppUser> userManager, ILogger<HomeController> logger, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
            _emailService = emailService;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {

            if (!ModelState.IsValid)
            {
                return View(request);

            }
            var identityResult = await _userManager.CreateAsync(new() { UserName = request.UserName, PhoneNumber = request.Phone, Email = request.Email }, request.PasswordConfirm);

            if (!identityResult.Succeeded)
            {
                ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());
                return View();
            }

            var exhangeExpireClaim = new Claim("ExchangeExpireDate", DateTime.Now.AddDays (10).ToString());

            var user = await _userManager.FindByNameAsync(request.UserName);

             var claimResult = await _userManager.AddClaimAsync(user!, exhangeExpireClaim) ;

            if (!claimResult.Succeeded)
            {
                ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());
                return View();
            }


            TempData["SuccessMessage"] = "Sign up is successful.";
            return RedirectToAction(nameof(SignUp));
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel request, string? returnUrl = null)
        {

            if (!ModelState.IsValid)
            {
                return View(request);

            }
            returnUrl ??= Url.Action("Index", "Home");
            var hasUser = await _userManager.FindByEmailAsync(request.Email);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email or password is incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(hasUser, request.Password, request.RememberMe, true);

            int entryRemain = await _userManager.GetAccessFailedCountAsync(hasUser);

            if (result.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "too many login attempts please try again later" });
                return View();
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(new List<string>() { $"Email or password is incorrect, (You have {3 - entryRemain} Entry left)" });
                return View();
            }

            if (hasUser.BirthDate.HasValue)
            {

                await _signInManager.SignInWithClaimsAsync(hasUser, request.RememberMe, new[] { new Claim("birthdate", hasUser.BirthDate.Value.ToString()) });
            }
            return Redirect(returnUrl!);

        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
        {
            var hasUser = await _userManager.FindByEmailAsync(request.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "No user found with this email address.");
                return View();
            }
            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);

            var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = hasUser.Id, Token = passwordResetToken }, HttpContext.Request.Scheme);

            await _emailService.SendResetPasswordEmail(passwordResetLink!, hasUser!.Email!);

            TempData["SuccessMessage"] = "Reset Password link has been sent to your mail address";

            return RedirectToAction(nameof(ForgetPassword));
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];

            if(userId == null || token == null)
            {
                throw new Exception("An Error handled");
            }

            var hasUser = await _userManager.FindByIdAsync(userId.ToString()!);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "User not found");
                return View();
            }
            var result = await _userManager.ResetPasswordAsync(hasUser, token.ToString()!, request.Password);

            if (result.Succeeded) {

                TempData["SuccessMessage"] = "Your password has been successfully renewed.";
            }
            else
            {
                ModelState.AddModelErrorList(result.Errors.Select(x => x.Description).ToList());
           
            }
            return View();
        }
    }
}
