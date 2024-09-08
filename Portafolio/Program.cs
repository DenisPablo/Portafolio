using Portafolio.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// A�adadimos un servicio Transient (Transitorio) a nuestro programa.
builder.Services.AddTransient<IRepositorioCategoria, RepositorioCategoria>();
builder.Services.AddTransient<IRepositorioUsuario, RepositorioUsuario>();

builder.Services.AddTransient<IRepositorioTecnologia, RepositorioTecnologia>();
builder.Services.AddTransient<IRepositorioProyecto, RepositorioProyecto>();

builder.Services.AddTransient<IRepositorioImagenProyecto, RepositorioImagenProyecto>();
builder.Services.AddTransient<ICloudinaryService, CloudinaryService>();

builder.Services.AddTransient<IProyectoUtilidades, ProyectoUtilidades>();

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

app.Use(async (context, next) =>
{
    try
    {
        await next(); // Procesa la solicitud
    }
    catch (Exception ex)
    {
        // Aqu� puedes registrar la excepci�n
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Se ha producido una excepci�n.");

        // Puedes devolver una respuesta personalizada
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("Ocurri� un error inesperado en el servidor.");
    }
});

