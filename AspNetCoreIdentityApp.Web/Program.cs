using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Core.OptionModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using AspNetCoreIdentityApp.Repository.Models;
using AspNetCoreIdentityApp.Repository.Seeds;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.ConfigureSqlConnectionExt(builder.Configuration);

builder.Services.Configure<SecurityStampValidatorOptions>(opt =>
{
    opt.ValidationInterval = TimeSpan.FromMinutes(30);
}); // Security Stamp


builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));


builder.Services.RegisterServicesExt(builder.Configuration);
builder.Services.RegisterPoliciesAndPermissionsExt();


builder.Services.ConfigureApplicationCookieExt();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    await PermissionSeed.Seed(roleManager);
}





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
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
