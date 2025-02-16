using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI;
using UserManagementAPI.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning(x =>
{
    x.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    x.AssumeDefaultVersionWhenUnspecified = true;
    x.ReportApiVersions = true;
    //x.ApiVersionReader = new QueryStringApiVersionReader("version");//urlversion
    //x.ApiVersionReader = new UrlSegmentApiVersionReader();
    //x.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
    x.ApiVersionReader = new MediaTypeApiVersionReader("v");
});

//builder.Services.AddMemoryCache();
//builder.Services.AddInMemoryRateLimiting();

//builder.Services.Configure<IpRateLimitOptions>(options =>
//{
//    options.EnableEndpointRateLimiting = true;
//    options.GeneralRules = new List<RateLimitRule>
//    {
//        new RateLimitRule
//        {
//            Endpoint = "*",
//            Limit = 3,// Max requests
//            Period = "10s"// Per minute
//        }
//    };
//});


//builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Configure Rate Limiting
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));

// Register required services for rate limiting
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

//  Add this line to fix the issue
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();


// Add GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<UserQuery>() // Register the Query Resolver
    .AddFiltering() // Enable Filtering
    .AddSorting();  // Enable Sorting

var app = builder.Build();

app.MapGraphQL(); // Exposes the GraphQL endpoint at "/graphql"

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<LoggingMiddleware>();

//app.UseIpRateLimiting();

// Use Middleware for Rate Limiting
app.UseIpRateLimiting();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
