using DIPTERV.Areas.Identity;
using DIPTERV.Context;
using DIPTERV.Data;
using DIPTERV.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Radzen;
using DIPTERV.Repositories;

var builder = WebApplication.CreateBuilder(args);

// database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// DI
//builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer());

// enable displaying database-related exceptions
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddScoped<NotificationService>();

//add repositories
builder.Services.AddScoped<TeacherRepository>();
builder.Services.AddScoped<SubjectDivisionRepository>();
builder.Services.AddScoped<SchoolClassRepository>();
builder.Services.AddScoped<RoomRepository>();
builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<TimeBlockRepository>();


//add services
builder.Services.AddScoped<TeacherService>();
builder.Services.AddScoped<SubjectDivisionService>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<SchoolClassService>();
builder.Services.AddScoped<GeneticAlgorithmService>();
builder.Services.AddScoped<FreeBlockService>();
builder.Services.AddScoped<TimetableService>();

//google authentication
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});

//Swagger services
/*
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DIPTERV API", Description = "TimeTable Scheduler For You", Version = "v1" });
});
*/

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//Swagger and Swagger UI endpoints
/*
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DIPTERV API V1");
});
*/

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
