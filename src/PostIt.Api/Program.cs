using PostIt.Api.Extensions;
using PostIt.Application.Abstractions.Auth;
using PostIt.Application.Abstractions.Services;
using PostIt.Application.Extensions;
using PostIt.Application.Services;
using PostIt.Infrastructure.Auth;
using PostIt.Infrastructure.Data.Configuration.Configurators;
using PostIt.Infrastructure.Data.Context;
using PostIt.Infrastructure.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Services.AddOpenApi();

services
    .AddSerilog(configuration)
    .AddDataAccess<ApplicationDbContext, ApplicationDbContextConfigurator>()
    .AddCachingDataAccess(configuration)
    .AddAuthenticationData(configuration)
    .AddApplication()
    .AddExceptionHandlers();

services.AddScoped<IAuthenticationService, AuthenticationService>();
services.AddScoped<IPasswordHasher, PasswordHasher>();
services.AddScoped<IJwtProvider, JwtProvider>();

var app = builder.Build();

app.MapApiEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();

app.Run();
