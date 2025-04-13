using PostIt.Infrastructure.Configuration.Configurators;
using PostIt.Infrastructure.Context;
using PostIt.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
services.AddOpenApi();
services.AddDataAccess<ApplicationDbContext, ApplicationDbContextConfigurator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
