using CoreIdentityStudy.Areas.Administrator.Models.AppRoles.PageVms;
using CoreIdentityStudy.Areas.Administrator.Models.AppRoles.ResponseModels;
using CoreIdentityStudy.Areas.Administrator.Models.AppUsers.RequestModels;
using CoreIdentityStudy.Models.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

namespace CoreIdentityStudy.Areas.Administrator.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administrator")]
    public class UserController : Controller
    {
        readonly UserManager<AppUser> _userManager;
        readonly RoleManager<AppRole> _roleManager;
        readonly IValidator<CreateUserRequestModel> _createUserValidator;

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IValidator<CreateUserRequestModel> createUserValidator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _createUserValidator = createUserValidator;
        }



        public IActionResult Index()
        {
            List<AppUser> allUsers = _userManager.Users.ToList();
            return View(allUsers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserRequestModel model)
        {
            ValidationResult validationResult = _createUserValidator.Validate(model);
            if (validationResult.IsValid)
            {
                AppUser appUser = new()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                };

                IdentityResult identityResult = await _userManager.CreateAsync(appUser, $"{model.UserName}cgr123");
                if (identityResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(appUser, "Member");
                    return RedirectToAction("Index");
                }
                foreach (IdentityError identityError in identityResult.Errors)
                {
                    ModelState.AddModelError("", identityError.Description);
                }
            }
            else
            {
                foreach (ValidationFailure validationFailure in validationResult.Errors)
                {

                    ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> AssignRole(int id)
        {
            AppUser appUser = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == id);

            IList<string> userRoles = await _userManager.GetRolesAsync(appUser);
            List<AppRole> allRoles = await _roleManager.Roles.ToListAsync();
            List<AppRoleResponseModel> responseRoles = new();

            foreach (AppRole item in allRoles)
            {
                responseRoles.Add(new()
                {
                    RoleName = item.Name,
                    IsChecked = userRoles.Contains(item.Name)
                });
            }

            AssignRolePageVM aRPVm = new()
            {
                UserID = id,
                Roles = responseRoles
            };

            return View(aRPVm);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(AssignRolePageVM pageModel)
        {
            AppUser appUser = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == pageModel.UserID);
            IList<string> userRoles = await _userManager.GetRolesAsync(appUser);

            foreach (AppRoleResponseModel item in pageModel.Roles)
            {
                if(!item.IsChecked && userRoles.Contains(item.RoleName))await _userManager.RemoveFromRoleAsync(appUser, item.RoleName);
                else if(item.IsChecked && !userRoles.Contains(item.RoleName)) await _userManager.AddToRoleAsync(appUser, item.RoleName);

            }

            return RedirectToAction("Index");   
        }
    }
}
