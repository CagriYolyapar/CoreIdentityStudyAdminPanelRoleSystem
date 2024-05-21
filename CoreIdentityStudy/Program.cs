using CoreIdentityStudy.Areas.Administrator.Models.AppRoles.RequestModels;
using CoreIdentityStudy.Areas.Administrator.Models.AppUsers.RequestModels;
using CoreIdentityStudy.Areas.Administrator.Models.FluentValidation.AppRoles;
using CoreIdentityStudy.Areas.Administrator.Models.FluentValidation.AppUsers;
using CoreIdentityStudy.Models.ContextClasses;
using CoreIdentityStudy.Models.Entities;
using CoreIdentityStudy.Models.FluentValidation.AppUsers;
using CoreIdentityStudy.Models.ViewModels.AppUsers.RequestModels;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<AppUser, AppRole>(x =>
{
    x.Password.RequiredLength = 3;
    x.Password.RequireLowercase = false;
    x.Password.RequireUppercase = false;
    x.Password.RequireNonAlphanumeric = false;
    x.Password.RequireDigit = false;
    x.SignIn.RequireConfirmedPhoneNumber = false;
    x.SignIn.RequireConfirmedEmail = false;
}).AddEntityFrameworkStores<MyContext>();


builder.Services.ConfigureApplicationCookie(x =>
{
    x.Cookie.HttpOnly = true;
    x.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    x.SlidingExpiration = true;
    x.Cookie.Name = "CyberAndRetro";
    x.Cookie.SameSite = SameSiteMode.Strict;
    x.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    x.LoginPath = new PathString("/Home/SignIn");
    x.AccessDeniedPath = new PathString("/Home/AccessDenied");
});

builder.Services.AddTransient<IValidator<UserRegisterRequestModel>, UserRegisterRequestModelValidator>();
builder.Services.AddTransient<IValidator<UserSignInRequestModel>, UserSignInRequestModelValidator>();
builder.Services.AddTransient<IValidator<CreateRoleRequestModel>, CreateRoleRequestModelValidator>();
builder.Services.AddTransient<IValidator<CreateUserRequestModel>,CreateUserRequestModelValidator>();    


builder.Services.AddDbContextPool<MyContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")).UseLazyLoadingProxies());

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "Administrator",
    pattern: "{area}/{controller}/{action}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Register}/{id?}");

app.Run();
