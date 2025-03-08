using NetBank.Core.Application;
using NetBank.Core.Application.Helpers;
using NetBank.Infraestructure.Identity;
using NetBank.Infraestructure.Persistence;
using NetBank.Infraestructure.Shared;
using NetBank.WebApp.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSession();
builder.Services.AddControllersWithViews();
builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.AddIdentityLayer(builder.Configuration);
builder.Services.AddSharedLayer(builder.Configuration);
builder.Services.AddApplicationLayer();
<<<<<<< HEAD
builder.Services.AddScoped<LoginAuthorize>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ValidateUserSessionHelper, ValidateUserSessionHelper>();
=======
>>>>>>> a9d59c8ac4528fbf2a5290c32e2bf7fd98eb6dd8



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

await app.Services.RunIdentitySeeds();

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

await app.RunAsync();
