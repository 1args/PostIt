using PostIt.Api.Extensions.DependencyInjection;
using PostIt.Application.Extensions;
using PostIt.Common.Extensions;
using PostIt.Hosts.Extensions.DependencyInjection;
using PostIt.Hosts.Extensions.Middleware;
using PostIt.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services
    .AddInfrastructure(configuration)
    .AddApplication()
    .AddCommon()
    .AddApi(configuration);

var app = builder.Build();

app.UseApiMiddlewares();

app.Run();
