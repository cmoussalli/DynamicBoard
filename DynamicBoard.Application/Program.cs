using DynamicBoard.Application;
using DynamicBoard.Application.Components;
using DynamicBoard.Application.DomainServices;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents();

builder.Services.AddRazorPages();
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

var encryptedKeySection = configuration.GetSection("EncryptedKeySection");
var encryptedKey = encryptedKeySection["EncryptedKey"];
Storage.EncryptionKey = encryptedKey;

var app = builder.Build();




app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
