using CoreIdentityStudy.Areas.Administrator.Models.AppRoles.ResponseModels;

namespace CoreIdentityStudy.Areas.Administrator.Models.AppRoles.PageVms
{
    public class AssignRolePageVM
    {
        public int UserID { get; set; }
        public List<AppRoleResponseModel> Roles { get; set; }
    }
}
