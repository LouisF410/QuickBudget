using QuickBudget.API;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices();

builder.Host.UseSerilog();

var app = builder
    .Build();

app.Configure(app.Environment, app.Logger);

app.Run();