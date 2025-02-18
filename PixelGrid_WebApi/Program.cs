using PixelGrid_WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

string connectionString = builder.Configuration["DB-ConnectionString"];

builder.Services.AddTransient<ISqlEnvironment2DService, SqlEnvironment2DService>(o => new SqlEnvironment2DService(connectionString));
builder.Services.AddTransient<ISqlObject2DService, SqlObject2DService>(o => new SqlObject2DService(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
