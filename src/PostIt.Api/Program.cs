using PostIt.Api.Extensions.DependencyInjection;
using PostIt.Api.Extensions.Middleware;
using PostIt.Application.Extensions;
using PostIt.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services
    .AddInfrastructure(configuration)
    .AddApplication()
    .AddApi(configuration);

var app = builder.Build();

app.UseApiMiddlewares();

app.Run();
