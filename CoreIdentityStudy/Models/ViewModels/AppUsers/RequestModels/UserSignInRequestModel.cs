﻿namespace CoreIdentityStudy.Models.ViewModels.AppUsers.RequestModels
{
    public class UserSignInRequestModel : IRegisterSignInSpec
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }

    }
}
