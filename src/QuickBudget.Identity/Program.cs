
using QuickBudget.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices();

var app = builder.Build();

app.Configure(app.Environment, app.Logger);

app.Run();
