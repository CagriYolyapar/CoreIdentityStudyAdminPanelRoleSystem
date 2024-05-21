using CoreIdentityStudy.Models.ViewModels.AppUsers.RequestModels;
using FluentValidation;

namespace CoreIdentityStudy.Models.FluentValidation.AppUsers
{
    public abstract class RegisterSignInSharedValidator<T> : AbstractValidator<T> where T:IRegisterSignInSpec
    {
        public RegisterSignInSharedValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı ismi bos gecilemez");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Sifre alanı bos gecilemez").MinimumLength(3).WithMessage("Parola minimum  3 karakterden olusmalıdır");
        }
    }
}
