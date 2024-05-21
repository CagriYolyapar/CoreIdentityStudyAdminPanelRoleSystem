using CoreIdentityStudy.Models.ViewModels.AppUsers.RequestModels;
using FluentValidation;

namespace CoreIdentityStudy.Models.FluentValidation.AppUsers
{
    public class UserRegisterRequestModelValidator : RegisterSignInSharedValidator<UserRegisterRequestModel>
    {
        public UserRegisterRequestModelValidator()
        {
          
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Parolalar uyusmuyor").NotEmpty().WithMessage("Sifre tekrar alanı gereklidir");
            RuleFor(x => x.Email).EmailAddress().NotEmpty().When(x => x.UserName == null).WithMessage("Email formatında giriş yapınız");
        }
    }
}
