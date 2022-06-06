using EnigmaApi.Infrastructure.SQL;
using EnigmaApi.Interfaces;
using EnigmaApi.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IRotorRepository, RotorRepository>();

//build connection to database
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseMySql(builder.Configuration.GetConnectionString("MySql"), 
        new MySqlServerVersion(ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql")))));


var app = builder.Build();

//Run ApplicationDbContext on Startup
var scope = app.Services.CreateScope();
scope.ServiceProvider.GetService<ApplicationDbContext>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
