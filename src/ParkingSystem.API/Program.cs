using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ParkingSystem.Infrastructure.Data;
using ParkingSystem.API.Services;
using QuestPDF.Infrastructure;

QuestPDF.Settings.License = LicenseType.Community;
var builder = WebApplication.CreateBuilder(args);


builder.WebHost.ConfigureKestrel(serverOptions =>
{
    
    serverOptions.ListenLocalhost(5163); 
  
    serverOptions.ListenLocalhost(7079, listenOptions => 
    {
        listenOptions.UseHttps(); 
    });
});

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger/OpenAPI 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Parking System API", 
        Version = "v1",
        Description = "API para gerenciamento de sistema de estacionamento"
    });
});

// Configurar Entity Framework com SQLite 
builder.Services.AddDbContext<ParkingDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IParkingService, ParkingService>();
builder.Services.AddScoped<IReportService, ReportService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("Development", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ParkingDbContext>();
    await context.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Parking System API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();


app.UseCors("Development");

app.UseAuthorization();

app.MapControllers();

app.Run();
