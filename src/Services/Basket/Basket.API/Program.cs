using BuildingBlocks.Messaging.MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();
builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
    configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
    configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});


builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks()
       .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
       .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
}).ConfigurePrimaryHttpMessageHandler(options =>
{
    var handler = new HttpClientHandler
                  {
                        ServerCertificateCustomValidationCallback = 
                            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                  };

    return handler;
});

builder.Services.AddMassTransitMessaging(builder.Configuration);
    
var app = builder.Build();

app.MapCarter();
app.UseExceptionHandler( opt => {});
app.UseHealthChecks("/health",
                    new HealthCheckOptions
                    {
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                        AllowCachingResponses = true
                    });

app.Run();