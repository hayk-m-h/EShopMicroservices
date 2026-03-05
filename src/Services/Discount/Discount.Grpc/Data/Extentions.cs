namespace Discount.Grpc.Data;

public static class Extentions
{
    public static async Task<IApplicationBuilder> UseMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<DiscountContext>();
        await context.Database.MigrateAsync();

        return app;
    }
}