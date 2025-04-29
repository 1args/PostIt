using Hangfire;
using PostIt.Api.Extensions.DependencyInjection;
using PostIt.Api.Extensions.Endpoints;
using PostIt.Application.Extensions;
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
    .AddAuthenticationRules(configuration)
    .AddSmtpConfiguration(configuration)
    .AddHangfireConfiguration(configuration)
    .AddApplication()
    .AddApi();

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

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard();

app.Run();
