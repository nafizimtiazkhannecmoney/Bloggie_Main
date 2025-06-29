using Bloggie.Web.Data;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Adding the DbContext to the services collection
builder.Services.AddDbContext<BloggieDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieDbConnectionString")));

// Registering the AuthDbContext with the DI container
builder.Services.AddDbContext<AuthDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieAuthDbConnectionString")));


// Configure the IdentityOptions to customize password requirements, such as disabling upper and lowercase requirements.
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password Settings
    options.Password.RequireDigit = false; // Require at least one digit
    options.Password.RequireLowercase = false; // Require at least one lowercase letter
    options.Password.RequireNonAlphanumeric = false; // Require at least one non-alphanumeric character
    options.Password.RequireUppercase = false; // Require at least one uppercase letter  
    options.Password.RequiredLength = 3; // Minimum length of the password
    options.Password.RequiredUniqueChars = 0; // Minimum number of unique characters in the password
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>();

builder.Services.AddScoped<ITagRepository, TagRepository>();                    // Registering the TagRepository with the DI container
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();         //  Registering the BlogPostRepository with the DI container
builder.Services.AddScoped<IImageRepository, CloudinaryImageRepository>();    //   Registering the CloudinaryImageRepository with the DI container 
builder.Services.AddScoped<IBlogPostLikeRepository, BlogPostLikeRepository>(); // Registering the BlogPostLikeRepository with the DI container

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

app.UseAuthentication(); // Adding authentication middleware to the pipeline
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
