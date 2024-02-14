using DynamicBoard.UnitTest;
using DynamicBoard.UnitTest.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

ConfigurationManager configuration = builder.Configuration;

var dynamicBoardSection = configuration.GetSection("DynamicBoardSection");
var encryptedKey = dynamicBoardSection["EncryptedKey"];
SME.DynamicBoard.UI.Storage.EncryptionKey = encryptedKey;
var rootAddress = dynamicBoardSection["DynamicBoardApplicationRootAddress"];
SME.DynamicBoard.UI.Storage.RootAddress = rootAddress;


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
