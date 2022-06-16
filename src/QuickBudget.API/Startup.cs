using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using QuickBudget.API.Infrastructure.AutofacModules;
using QuickBudget.API.Infrastructure.Filters;
using QuickBudget.Persistance;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace QuickBudget.API;

public static class Startup
{
    public static IConfiguration Configuration => GetConfiguration();

    public static IServiceProvider ConfigureServices(this IServiceCollection services)
    {
        services
            .AddApplicationInsights(Configuration)
            .AddCustomMvc()
            //.AddHealthChecks(Configuration)
            .AddCustomDbContext(Configuration)
            .AddCustomSwagger(Configuration)
            .AddCustomIntegrations(Configuration)
            .AddCustomConfiguration(Configuration)
            //.AddEventBus(Configuration)
            .AddCustomAuthentication(Configuration);

        var container = new ContainerBuilder();
        container.Populate(services);

        container.RegisterModule(new MediatorModule());
        //container.RegisterModule(new ApplicationModule(Configuration["ConnectionString"]));

        return new AutofacServiceProvider(container.Build());
    }

    public static void Configure(this IApplicationBuilder app, IWebHostEnvironment env, ILogger logger)
    {
        var pathBase = Configuration["PATH_BASE"];
        if (!string.IsNullOrEmpty(pathBase))
        {
            logger.LogDebug("Using PATH BASE '{pathBase}'", pathBase);
            //loggerFactory.CreateLogger(typeof(Startup)).LogDebug("Using PATH BASE '{pathBase}'", pathBase);
            app.UsePathBase(pathBase);
        }

        app.UseSwagger()
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{(!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty)}/swagger/v1/swagger.json", "Ordering.API V1");
                c.OAuthClientId("orderingswaggerui");
                c.OAuthAppName("Ordering Swagger UI");
            });

        app.UseRouting();
        app.UseCors("CorsPolicy");
        ConfigureAuth(app);

        app.UseEndpoints(endpoints =>
        {
            //endpoints.MapGrpcService<OrderingService>();
            endpoints.MapDefaultControllerRoute();
            endpoints.MapControllers();
            //endpoints.MapGet("/_proto/", async ctx =>
            //{
            //    ctx.Response.ContentType = "text/plain";
            //    using var fs = new FileStream(Path.Combine(env.ContentRootPath, "Proto", "basket.proto"), FileMode.Open, FileAccess.Read);
            //    using var sr = new StreamReader(fs);
            //    while (!sr.EndOfStream)
            //    {
            //        var line = await sr.ReadLineAsync();
            //        if (line != "/* >>" || line != "<< */")
            //        {
            //            await ctx.Response.WriteAsync(line);
            //        }
            //    }
            //});
            //endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
            //{
            //    Predicate = _ => true,
            //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            //});
            //endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
            //{
            //    Predicate = r => r.Name.Contains("self")
            //});
        });
    }

    static void ConfigureAuth(IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }

    static IConfiguration GetConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        var config = builder.Build();

        return builder.Build();
    }
}

static class CustomExtensionsMethods
{
    public static IServiceCollection AddApplicationInsights(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationInsightsTelemetry(configuration);
        //services.AddApplicationInsightsKubernetesEnricher();

        return services;
    }

    public static IServiceCollection AddCustomMvc(this IServiceCollection services)
    {
        // Add framework services.
        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(HttpGlobalExceptionFilter));
        })
        // Added for functional tests
        .AddApplicationPart(typeof(Startup).Assembly)
        .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder
                .SetIsOriginAllowed((host) => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
        });

        return services;
    }

    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<QuickBudgetDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("Default"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    }));

        return services;
    }

    public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "QuickBudget HTTP API",
                Version = "v1",
                Description = "QuickBudget Service HTTP API"
            });
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows()
                {
                    Implicit = new OpenApiOAuthFlow()
                    {
                        AuthorizationUrl = new Uri($"{configuration.GetValue<string>("IdentityUrlExternal")}/connect/authorize"),
                        TokenUrl = new Uri($"{configuration.GetValue<string>("IdentityUrlExternal")}/connect/token"),
                        Scopes = new Dictionary<string, string>()
                        {
                            { "api", "QuickBudget API" }
                        }
                    }
                }
            });

            options.OperationFilter<AuthorizeCheckOperationFilter>();
        });

        return services;
    }

    public static IServiceCollection AddCustomIntegrations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //services.AddTransient<IIdentityService, IdentityService>();
        //services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
        //    sp => (DbConnection c) => new IntegrationEventLogService(c));

        //services.AddTransient<IOrderingIntegrationEventService, OrderingIntegrationEventService>();

        //if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
        //{
        //    services.AddSingleton<IServiceBusPersisterConnection>(sp =>
        //    {
        //        var serviceBusConnectionString = configuration["EventBusConnection"];

        //        var subscriptionClientName = configuration["SubscriptionClientName"];

        //        return new DefaultServiceBusPersisterConnection(serviceBusConnectionString);
        //    });
        //}
        //else
        //{
        //    services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
        //    {
        //        var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();


        //        var factory = new ConnectionFactory()
        //        {
        //            HostName = configuration["EventBusConnection"],
        //            DispatchConsumersAsync = true
        //        };

        //        if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
        //        {
        //            factory.UserName = configuration["EventBusUserName"];
        //        }

        //        if (!string.IsNullOrEmpty(configuration["EventBusPassword"]))
        //        {
        //            factory.Password = configuration["EventBusPassword"];
        //        }

        //        var retryCount = 5;
        //        if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
        //        {
        //            retryCount = int.Parse(configuration["EventBusRetryCount"]);
        //        }

        //        return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
        //    });
        //}

        return services;
    }

    public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        //services.Configure<OrderingSettings>(configuration);
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Please refer to the errors property for additional details."
                };

                return new BadRequestObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json", "application/problem+xml" }
                };
            };
        });

        return services;
    }

    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        //if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
        //{
        //    services.AddSingleton<IEventBus, EventBusServiceBus>(sp =>
        //    {
        //        var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
        //        var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
        //        var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
        //        var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
        //        string subscriptionName = configuration["SubscriptionClientName"];

        //        return new EventBusServiceBus(serviceBusPersisterConnection, logger,
        //            eventBusSubcriptionsManager, iLifetimeScope, subscriptionName);
        //    });
        //}
        //else
        //{
        //    services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
        //    {
        //        var subscriptionClientName = configuration["SubscriptionClientName"];
        //        var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
        //        var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
        //        var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
        //        var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

        //        var retryCount = 5;
        //        if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
        //        {
        //            retryCount = int.Parse(configuration["EventBusRetryCount"]);
        //        }

        //        return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
        //    });
        //}

        //services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

        return services;
    }

    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // prevent from mapping "sub" claim to nameidentifier.
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

        var identityUrl = configuration.GetValue<string>("IdentityUrl");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            options.Authority = identityUrl;
            options.RequireHttpsMetadata = false;
            options.Audience = "api";
        });

        return services;
    }
}