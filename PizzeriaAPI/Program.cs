global using PizzeriaAPI.Models;
using Microsoft.EntityFrameworkCore;
using PizzeriaAPI.Data;
using PizzeriaAPI.Services.IngredientService;
using PizzeriaAPI.Services.PizzaService;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Memory;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PizzeriaContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"))
);
builder.Services.AddScoped<IPizzaService, PizzaService>();
builder.Services.AddScoped<IngredientService>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IIngredientService, IngredientCache>();
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
var app = builder.Build();

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
