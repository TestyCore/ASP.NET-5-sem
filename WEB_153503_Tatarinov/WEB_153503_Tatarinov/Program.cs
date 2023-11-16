using WEB_153503_Tatarinov.Models;
using WEB_153503_Tatarinov.Services.CategoryService;
using WEB_153503_Tatarinov.Services.ProductService;
using Microsoft.AspNetCore.Authentication;
using NuGet.Packaging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

UriData uriData = builder.Configuration.GetSection("UriData").Get<UriData>()!;

builder.Services
    .AddHttpClient<IProductService, ApiProductService>(opt=>
        opt.BaseAddress=new Uri(uriData.ApiUri));

builder.Services
    .AddHttpClient<ICategoryService, ApiCategoryService>(opt=>
        opt.BaseAddress=new Uri(uriData.ApiUri));


builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultScheme = "cookie";
        opt.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("cookie")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority =
            builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
        options.ClientId =
            builder.Configuration["InteractiveServiceSettings:ClientId"];
        options.ClientSecret =
            builder.Configuration["InteractiveServiceSettings:ClientSecret"];
        // Получить Claims пользователя
        options.GetClaimsFromUserInfoEndpoint = true;
        options.ResponseType = "code";
        options.ResponseMode = "query";
        options.SaveTokens = true;
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
app.UseStaticFiles();

app.UseRouting();
app.MapRazorPages().RequireAuthorization();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();