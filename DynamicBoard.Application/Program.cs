using CMouss.IdentityFramework;
using DynamicBoard.Application;
using DynamicBoard.Application.Components;
using DynamicBoard.Application.DomainServices;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents();

builder.Services.AddControllersWithViews();

// temporarly commented builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();


ConfigurationManager configuration = builder.Configuration;
var DataBaseConnectionSection = configuration.GetSection("ConnectionStringsSection");
var Connectionstring = DataBaseConnectionSection["DefaultConnectionDev"];
if (System.IO.File.Exists(@"C:\production.inf"))
{
    Connectionstring = DataBaseConnectionSection["DefaultConnectionProd"];
    StaticStorage.IsProduction = true;
}


Storage.DBConnectionString = Connectionstring;

IDFManager.Configure(new IDFManagerConfig
{

    DatabaseType = DatabaseType.MSSQL,
    DBConnectionString = Storage.DBConnectionString,
    DefaultListPageSize = 25,
    DBLifeCycle = DBLifeCycle.Both,
    IsActiveByDefault = true,
    IsLockedByDefault = false,
    DefaultTokenLifeTime = new LifeTime(30, 0, 0)
});

var encryptedKeySection = configuration.GetSection("EncryptedKeySection");
var encryptedKey = encryptedKeySection["EncryptedKey"];
Storage.EncryptionKey = encryptedKey;

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.UseAntiforgery();

app.MapRazorComponents<DynamicBoard.Application.Components.App>()
    .AddInteractiveServerRenderMode();

app.MapControllerRoute(
name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");
app.Run();
