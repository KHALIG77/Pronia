using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(opt => opt.IdleTimeout = TimeSpan.FromHours(5));


builder.Services.AddDbContext<ProniaContext>(opt =>
{
opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddScoped<LayoutService>();
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
	opt.Password.RequireNonAlphanumeric = false;
	opt.Password.RequireUppercase = false;
	opt.Password.RequiredLength = 6;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<ProniaContext>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddAuthentication().AddGoogle(opt =>
{
    opt.ClientId = "392486084712-ri54hd00he20ulikkatq039k5i6hfg3v.apps.googleusercontent.com";
    opt.ClientSecret = "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";
});
builder.Services.ConfigureApplicationCookie(opt =>
{
	opt.Events.OnRedirectToLogin = opt.Events.OnRedirectToAccessDenied = context =>
	{
		if (context.HttpContext.Request.Path.Value.StartsWith("/manage"))
		{
			var redirectUri=new Uri(context.RedirectUri);
            context.Response.Redirect("/manage/account/login"+redirectUri.Query);

		}
		else
		{
			var redirectUri = new Uri(context.RedirectUri);
			context.Response.Redirect("/account/login" + redirectUri.Query);
		}
		
        return Task.CompletedTask;
    };
	
});
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
