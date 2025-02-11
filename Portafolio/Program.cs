using Microsoft.AspNetCore.Identity;
using Portafolio.Models;
using Portafolio.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Añadadimos un servicio Transient (Transitorio) a nuestro programa.
builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();

builder.Services.AddScoped<IRepositorioTecnologia, RepositorioTecnologia>();
builder.Services.AddScoped<IRepositorioProyecto, RepositorioProyecto>();

builder.Services.AddScoped<IRepositorioImagenProyecto, RepositorioImagenProyecto>();
builder.Services.AddTransient<ICloudinaryService, CloudinaryService>();

builder.Services.AddTransient<IProyectoUtilidades, ProyectoUtilidades>();
builder.Services.AddTransient<IRepositorioTecnologiaUsada, RepositorioTecnologiaUsada>();

builder.Services.AddScoped<IUserStore<Usuario>, UsuarioStore>();
builder.Services.AddIdentityCore<Usuario>().AddErrorDescriber<MensajesDeErrorIdentity>();


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();

