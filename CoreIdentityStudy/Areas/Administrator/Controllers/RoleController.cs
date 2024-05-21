using CoreIdentityStudy.Areas.Administrator.Models.AppRoles.RequestModels;
using CoreIdentityStudy.Models.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace CoreIdentityStudy.Areas.Administrator.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administrator")]
    public class RoleController : Controller
    {
        readonly RoleManager<AppRole> _roleManager;
        readonly IValidator<CreateRoleRequestModel> _createRoleValidator;



        public RoleController(RoleManager<AppRole> roleManager, IValidator<CreateRoleRequestModel> createRoleValidator)
        {
            _roleManager = roleManager;
            _createRoleValidator = createRoleValidator;
        }

        public IActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleRequestModel model)
        {
            ValidationResult validationResult = _createRoleValidator.Validate(model);
            if (validationResult.IsValid)
            {
                IdentityResult identityResult = await _roleManager.CreateAsync(new() { Name = model.RoleName });
                if (identityResult.Succeeded)
                {
                    return RedirectToAction("Index");
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
                    ModelState.AddModelError(validationError.PropertyName,validationError.ErrorMessage);
                }
            }

            return View(model);
        }
    }
}
