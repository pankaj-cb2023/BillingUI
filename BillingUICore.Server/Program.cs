using BillingUI.Business.IService;
using BillingUI.Business.Services;
using BillingUI.Data.IRepository;
using BillingUI.Data.Repository;
using BillingUI.Settings;
using BillingUICore.Server.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Web;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder => builder.WithOrigins("https://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
});

//// Add authentication
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

//Enable window Authentication
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

//connection string
var connectionString = builder.Configuration.GetConnectionString("BillingUIConnection") ?? string.Empty;

// Example: Using ADO.NET to create a SqlConnection
builder.Services.AddSingleton<IDbConnection>(new SqlConnection(connectionString));

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<CoreImplementation>();
builder.Services.AddScoped<BaseImplementation>();

// Serices
builder.Services.AddTransient<IRatePlanService, RatePlanService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IEventService, EventService>();


//Repository
builder.Services.AddTransient<IRatePlanRepository, RatePlanRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IEventRepository, EventRepository>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();


var app = builder.Build();

app.UseCors("AllowLocalhost");

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
