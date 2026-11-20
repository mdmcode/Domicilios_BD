using DomiciliosApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    // Mensajes de validación en español
    var m = options.ModelBindingMessageProvider;
    m.SetValueMustNotBeNullAccessor(_ => "Este campo es obligatorio.");
    m.SetMissingBindRequiredValueAccessor(campo => $"Falta el valor de {campo}.");
    m.SetAttemptedValueIsInvalidAccessor((valor, campo) => $"'{valor}' no es un valor válido para {campo}.");
    m.SetUnknownValueIsInvalidAccessor(campo => $"El valor no es válido para {campo}.");
    m.SetValueIsInvalidAccessor(valor => $"'{valor}' no es válido.");
    m.SetValueMustBeANumberAccessor(campo => $"{campo} debe ser un número.");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

var cultura = CultureInfo.InvariantCulture;
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultura),
    SupportedCultures = new[] { cultura },
    SupportedUICultures = new[] { cultura }
});

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
