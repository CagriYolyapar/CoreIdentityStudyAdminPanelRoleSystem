﻿namespace CoreIdentityStudy.Models.ViewModels.AppUsers.RequestModels
{
    public class UserRegisterRequestModel : IRegisterSignInSpec
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? Email { get; set; }

    }
}
