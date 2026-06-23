using Microsoft.EntityFrameworkCore;
using VisionNaranja.Data;
using VisionNaranja.Services;

var builder = WebApplication.CreateBuilder(args);

// Servicios MVC / Razor Pages
builder.Services.AddRazorPages();

// Registro del DbContext usando la cadena de conexión de appsettings.json
builder.Services.AddDbContext<VisionNaranjaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VisionNaranjaConnection")));
builder.Services.AddScoped<CatalogoService>();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();