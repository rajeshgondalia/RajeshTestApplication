using ReqresApi.Client.Interfaces;
using ReqresApi.Client.Services;
using ReqresApi.Client;
using ReqresApi.Client.Configuration;
using ReqresApi.Client.Caching;
using Polly.Extensions.Http;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.  
builder.Services.AddControllersWithViews();

// Fix for CS0103: Use 'builder.Services' instead of 'services'  
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));
builder.Services.AddMemoryCache();

builder.Services.Configure<ReqresApiClientOptions>(builder.Configuration.GetSection("ReqresApi"));
builder.Services.Configure<CachingOptions>(builder.Configuration.GetSection("Caching"));
 

//builder.Services.AddHttpClient<IReqresApiClient, ReqresApiClient>()
//   .AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError()
//       .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(500)));

builder.Services.AddMemoryCache(); // Adds IMemoryCache  

builder.Services.AddHttpClient<IReqresApiClient, ReqresApiClient>();
builder.Services.AddTransient<ExternalUserService>();

// Configure the HTTP request pipeline.  
var app = builder.Build();
var svc = app.Services.GetRequiredService<ExternalUserService>();

var user = await svc.GetUserByIdAsync(2);
Console.WriteLine($"{user?.FirstName} {user?.LastName}");

// Configure the HTTP request pipeline.  
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
   name: "default",
   pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
