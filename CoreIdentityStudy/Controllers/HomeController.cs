using CoreIdentityStudy.Models;
using CoreIdentityStudy.Models.Entities;
using CoreIdentityStudy.Models.ViewModels.AppUsers.RequestModels;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace CoreIdentityStudy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly UserManager<AppUser> _userManager;
        readonly RoleManager<AppRole> _roleManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly IValidator<UserRegisterRequestModel> _userRegisterRequestValidator;
        readonly IValidator<UserSignInRequestModel> _userSignInRequestValidator;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IValidator<UserRegisterRequestModel> userRegisterRequestValidator, IValidator<UserSignInRequestModel> userSignInRequestValidator, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _userRegisterRequestValidator = userRegisterRequestValidator;
            _userSignInRequestValidator = userSignInRequestValidator;
            _signInManager = signInManager;
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

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterRequestModel model)
        {
            ValidationResult validationResult = _userRegisterRequestValidator.Validate(model);
            if(validationResult.IsValid)
            {
                AppUser appUser = new()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                };

                IdentityResult identityResult = await _userManager.CreateAsync(appUser, model.Password);
                if (identityResult.Succeeded)
                {
                    #region AdminEklemek
                    //if (await _roleManager.FindByNameAsync("Admin") == null) await _roleManager.CreateAsync(new() { Name = "Admin" });
                    //await _userManager.AddToRoleAsync(appUser, "Admin");
                    #endregion

                    #region MemberEklemek
                    if (await _roleManager.FindByNameAsync("Member") == null) await _roleManager.CreateAsync(new() { Name = "Member" });
                    await _userManager.AddToRoleAsync(appUser, "Member");
                    #endregion

                    TempData["message"] = $"{appUser.UserName} isimli kullanıcı basarıyla kayıt oldu";
                    return RedirectToAction("Register");
                }

                foreach (IdentityError identityError in identityResult.Errors)
                {
                    ModelState.AddModelError("", identityError.Description);

                }
               
            }
            else
            {
                foreach (ValidationFailure validationError in validationResult.Errors)
                {
                    ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                }
            }
            return View(model);

        }


        public IActionResult SignIn(string? returnUrl)
        {
            UserSignInRequestModel usModel = new()
            {
                ReturnUrl = returnUrl
            };
            return View(usModel);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInRequestModel model)
        {
            ValidationResult validationResult = _userSignInRequestValidator.Validate(model);
            if(validationResult.IsValid)
            {
                AppUser appUser =await _userManager.FindByNameAsync(model.UserName);
                if(appUser == null)
                {
                    TempData["message"] = "Kullanıcı bulunamadı";
                    return RedirectToAction("SignIn");
                }
                SignInResult signInResult =await _signInManager.PasswordSignInAsync(appUser, model.Password,model.RememberMe,true);

                if(signInResult.Succeeded) 
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    IList<string> userRoles = await _userManager.GetRolesAsync(appUser);
                    if (userRoles.Contains("Admin")) return RedirectToAction("AdminPanel", "Auth");
                    else if (userRoles.Contains("Member")) return RedirectToAction("MemberPanel");
                    return RedirectToAction("Panel");
                }
                else if (signInResult.IsLockedOut)
                {
                    DateTimeOffset? lockOutEndDate = await _userManager.GetLockoutEndDateAsync(appUser);
                    ModelState.AddModelError("", $"Hesabınız {(lockOutEndDate.Value.UtcDateTime - DateTime.UtcNow).Minutes} dakika süreyle askıya alınmıstır");
                }

                else
                {
                    int maxFailedAttempts = _userManager.Options.Lockout.MaxFailedAccessAttempts;
                    string message = $"Eger {maxFailedAttempts-await _userManager.GetAccessFailedCountAsync(appUser)} kez daha yanlıs giriş yaparsanız hesabınız gecici olarak askıya alınacaktır";

                    ModelState.AddModelError("", message);
                }
            }
            else
            {
                foreach (ValidationFailure validationError in validationResult.Errors)
                {
                    ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                }
            }
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles ="Member")]
        public IActionResult MemberPanel()
        {
            return View();
        }

        public IActionResult Panel()
        {
            return View();
        }
    }
}
