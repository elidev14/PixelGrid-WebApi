using Microsoft.AspNetCore.Identity;
using PixelGrid_WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

string connectionString = builder.Configuration["SQLConnectionString"];
bool connectionStringFound = !string.IsNullOrEmpty(connectionString);

builder.Services.AddTransient<ISqlEnvironment2DService, SqlEnvironment2DService>(o => new SqlEnvironment2DService(connectionString));
builder.Services.AddTransient<ISqlObject2DService, SqlObject2DService>(o => new SqlObject2DService(connectionString));
// Adding the HTTP Context accessor to be injected. This is needed by the AspNetIdentityUserRepository
// to resolve the current user.
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAuthenticationService, AspNetIdentityAuthenticationService>();

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.Password.RequiredLength = 50;
})
.AddRoles<IdentityRole>()
.AddDapperStores(options =>
{
    options.ConnectionString = connectionString;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapGet("/", () => $"Api is up! Connecection string found: {(connectionStringFound ? '✅' : '❌')}");

app.MapGroup("/account").MapIdentityApi<IdentityUser>();

app.UseHttpsRedirection();


app.MapControllers().RequireAuthorization();


app.Run();
