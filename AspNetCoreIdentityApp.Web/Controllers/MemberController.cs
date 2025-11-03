using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;

        public MemberController(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();

        }
    }
}
