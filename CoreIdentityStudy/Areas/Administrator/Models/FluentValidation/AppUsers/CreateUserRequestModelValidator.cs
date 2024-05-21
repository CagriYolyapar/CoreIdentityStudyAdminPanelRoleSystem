using CoreIdentityStudy.Areas.Administrator.Models.AppUsers.RequestModels;
using FluentValidation;

namespace CoreIdentityStudy.Areas.Administrator.Models.FluentValidation.AppUsers
{
    public class CreateUserRequestModelValidator : AbstractValidator<CreateUserRequestModel>
    {
        public CreateUserRequestModelValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı ismi bos gecilemez");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email alanı bos gecilemez").EmailAddress().WithMessage("Lutfen email formatını dogru giriniz");
        }
    }
}
