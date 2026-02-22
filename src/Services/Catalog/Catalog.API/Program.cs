using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddCarter();
builder.Services.AddMediatR( configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
    configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

var app = builder.Build();

app.MapCarter();

app.UseExceptionHandler( app =>
{
    app.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;

        if (exception is null)
        {
            return;
        }

        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception, message: exception?.Message);

        context.Response.ContentType = "application/problem+json";

        if (exception is ValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                Errors = validationException.Errors.Select(e => new
                {
                    e.PropertyName,
                    e.ErrorMessage
                })
            });
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new
            {
                Title = exception?.Message,
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception?.StackTrace
            });
        }
    });
});

app.Run();
