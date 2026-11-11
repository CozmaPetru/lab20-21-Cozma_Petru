using System.Globalization;
using LibrarieLab.Data;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// S1: aici înregistrezi contextul bazei de date
builder.Services.AddDbContext<LibrarieContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("Librarie")));

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Prețul se scrie cu punct (129.90), oricare ar fi setările regionale ale Windows
CultureInfo cultura = new CultureInfo("en-US");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultura),
    SupportedCultures = new List<CultureInfo> { cultura },
    SupportedUICultures = new List<CultureInfo> { cultura }
});

app.UseHttpsRedirection();

// Fișierele încărcate în timpul rulării (wwwroot/incarcari), necesar în aplicația publicată
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Carti}/{action=Carduri}/{id?}")
    .WithStaticAssets();
    
app.Run();
