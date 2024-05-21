using CoreIdentityStudy.Areas.Administrator.Models.AppRoles.RequestModels;
using FluentValidation;

namespace CoreIdentityStudy.Areas.Administrator.Models.FluentValidation.AppRoles
{
    public class CreateRoleRequestModelValidator : AbstractValidator<CreateRoleRequestModel>
    {
        public CreateRoleRequestModelValidator()
        {
            RuleFor(x => x.RoleName).NotEmpty().WithMessage("Rol ismi bos gecilemez");
        }
    }
}
