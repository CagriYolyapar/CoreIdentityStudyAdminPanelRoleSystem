using CoreIdentityStudy.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace CoreIdentityStudy.Areas.Administrator.CustomTagHelpers
{
    //Bu Helper'in amacı aldıgı kullanıcı nesnesinin rollerini tespit etmek ve rol isimlerini bir htmlString'e cıkarmak olacak
    [HtmlTargetElement("getUserRoles")]
    public class AdminCustomRolesHelper : TagHelper
    {

        readonly UserManager<AppUser> _userManager;

        public AdminCustomRolesHelper(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public int UserID { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            string html = "";
            IList<string> userRoles = await _userManager.GetRolesAsync(await _userManager.Users.SingleOrDefaultAsync(x => x.Id == UserID));

            foreach (string role in userRoles)
            {
                html += $"{role},";
            }

            html = html.TrimEnd(',');

            output.Content.SetHtmlContent(html);
        }
    }
}
