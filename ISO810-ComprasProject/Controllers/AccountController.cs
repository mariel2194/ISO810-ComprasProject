using ISO810_ComprasProject.Data;
using ISO810_ComprasProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace ISO810_ComprasProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly ComprasDBContext _appDbContext;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        public AccountController(ComprasDBContext comprasDBContext, UserManager<Users> userManager, SignInManager<Users> signInManager)
        {
            this._appDbContext = comprasDBContext; 
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if(User.Identity.IsAuthenticated) return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace("Controller", ""));
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Correo o contraseña incorrectos.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {

                if (model.Password != model.ConfirmarPassword)
                {
                    ViewData["Mensaje"] = "Las contraseñas no coinciden";
                    return View();
                }

                var user = new Users { Email = model.Email, Nombre = model.Nombre, Apellido = model.Apellido, UserName = model.Email, Password = model.Password };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
            else
            {
                ViewData["Mensaje"] = "Complete todo los valores";
            }

            return View();
        }

        public async Task<IActionResult> LogOut() 
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(AccountController.Login),nameof(AccountController).Replace("Controller", "")); 
        }

    }
}
