using CoreIdentityStudy.Models.ViewModels.AppUsers.RequestModels;
using FluentValidation;

namespace CoreIdentityStudy.Models.FluentValidation.AppUsers
{
    public class UserSignInRequestModelValidator : RegisterSignInSharedValidator<UserSignInRequestModel>
    {
        public UserSignInRequestModelValidator()
        {
          
        }
    }
}
