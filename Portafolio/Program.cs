using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Portafolio.Models;
using Portafolio.Servicios;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())  // Esto es para asegurarte que se cargue el archivo correctamente
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)  // Asegúrate de que appsettings.json esté siendo cargado
    .AddEnvironmentVariables();  // Agregar las variables de entorno

// Verificar si las variables se leen correctamente
Console.WriteLine("DB_SERVER: " + builder.Configuration["DB_SERVER"]);
Console.WriteLine("DB_NAME: " + builder.Configuration["DB_NAME"]);
Console.WriteLine("DB_USER: " + builder.Configuration["DB_USER"]);
Console.WriteLine("DB_PASSWORD: " + builder.Configuration["DB_PASSWORD"]);

// Add services to the container.

var politicaDeSeguridad = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AuthorizeFilter(politicaDeSeguridad))
;
});
builder.Services.AddHttpContextAccessor();

// A�adadimos un servicio Transient (Transitorio) a nuestro programa.
builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();

builder.Services.AddScoped<IRepositorioTecnologia, RepositorioTecnologia>();
builder.Services.AddScoped<IRepositorioProyecto, RepositorioProyecto>();

builder.Services.AddScoped<IRepositorioImagenProyecto, RepositorioImagenProyecto>();
builder.Services.AddTransient<ICloudinaryService, CloudinaryService>();

builder.Services.AddTransient<IProyectoUtilidades, ProyectoUtilidades>();
builder.Services.AddTransient<IRepositorioTecnologiaUsada, RepositorioTecnologiaUsada>();

builder.Services.AddScoped<IHtmlHelper, HtmlHelper>();

builder.Services.AddScoped<IUserStore<Usuario>, UsuarioStore>();
builder.Services.AddIdentityCore<Usuario>().AddErrorDescriber<MensajesDeErrorIdentity>();

builder.Services.AddTransient<SignInManager<Usuario>>();
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    option.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    option.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
}).AddCookie(IdentityConstants.ApplicationScheme, options =>
{
    options.LoginPath = "/Home/Index";
});

builder.Services.AddScoped<IServicioUsuario, ServicioUsuario>();

builder.Services.AddScoped<IDbConnection, Dbconnection>();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();

