namespace CoreIdentityStudy.Models.ViewModels.AppUsers.RequestModels
{
    public interface IRegisterSignInSpec
    {
       string? UserName { get; set; }
       string? Password { get; set; }
    }
}
