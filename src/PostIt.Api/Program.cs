using PostIt.Api.Extensions;
using PostIt.Application.Extensions;
using PostIt.Infrastructure.Data.Configuration.Configurators;
using PostIt.Infrastructure.Data.Context;
using PostIt.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Services.AddOpenApi();

services
    .AddDataAccess<ApplicationDbContext, ApplicationDbContextConfigurator>()
    .AddCachingDataAccess(configuration)
    .AddApplication()
    .AddSerilog(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
