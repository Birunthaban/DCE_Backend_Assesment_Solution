
using DCE_Backend_Developer_Assesment.Repositories;
using DCE_Backend_Developer_Assesment.Services;
using Microsoft.Extensions.Configuration; // Add this using statement

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register your services and repositories
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Configure application settings
builder.Configuration.AddJsonFile("appsettings.json"); // Load settings from appsettings.json
// You can add more configuration sources as needed (e.g., environment variables)

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
