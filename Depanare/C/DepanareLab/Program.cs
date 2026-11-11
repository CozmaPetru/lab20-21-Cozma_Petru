using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.EntityFrameworkCore;
using DepanareLab.Data;
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Literele românești rămân necodificate în HTML-ul trimis.
builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(UnicodeRanges.All));

// Contextul bazei de date: SQLite, cu șirul de conexiune din appsettings.json.
string sirConexiune = builder.Configuration.GetConnectionString("DepanareLab");

WebApplication app = builder.Build();

// Cultura aplicației: aceeași pe toate calculatoarele din laborator.
CultureInfo cultura = new CultureInfo("ro-MD");
CultureInfo.DefaultThreadCurrentCulture = cultura;
CultureInfo.DefaultThreadCurrentUICulture = cultura;

// Datele de start se scriu o singură dată, dacă tabelele sunt goale.
DateInitiale.Scrie(app.Services);

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

// Fișierele încărcate apar după pornire, deci este nevoie și de UseStaticFiles.
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Echipamente}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
