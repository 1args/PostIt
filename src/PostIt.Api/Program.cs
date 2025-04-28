using FluentValidation;
using Hangfire;
using PostIt.Api.Extensions;
using PostIt.Api.Extensions.DependencyInjection;
using PostIt.Api.Extensions.Endpoints;
using PostIt.Application.Abstractions.Auth;
using PostIt.Application.Abstractions.Services;
using PostIt.Application.Extensions;
using PostIt.Application.Services;
using PostIt.Infrastructure.Auth;
using PostIt.Infrastructure.Communication.Email;
using PostIt.Infrastructure.Data.Configuration.Configurators;
using PostIt.Infrastructure.Data.Context;
using PostIt.Infrastructure.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddOpenApi();
services.AddHttpContextAccessor();

services
    .AddExceptionHandlers()
    .AddSerilog(configuration)
    .AddDataAccess<ApplicationDbContext, ApplicationDbContextConfigurator>()
    .AddCachingDataAccess(configuration)
    .AddAuthenticationData(configuration)
    .AddSmtpConfiguration(configuration)
    .AddHangfireConfiguration(configuration)
    .AddApplication();

// Later I will move it to extensions
services.AddScoped<IAuthenticationService, AuthenticationService>();
services.AddScoped<IEmailService, EmailService>();
services.AddScoped<IPasswordHasher, PasswordHasher>();
services.AddScoped<IJwtProvider, JwtProvider>();
services.AddScoped<IEmailVerificationLinkFactory, EmailVerificationLinkFactory>();

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

app.UseHangfireDashboard();

app.Run();
