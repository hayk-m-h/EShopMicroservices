namespace Ordering.Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(c => c.Id).HasConversion(
                                                  id => id!.Value,
                                                  dbId => OrderItemId.Of(dbId));

        builder.HasOne<Product>()
               .WithMany()
               .HasForeignKey(k => k.ProductId);

        builder.Property(oi => oi.Quantity).IsRequired();
        builder.Property(oi => oi.Price).IsRequired();
    }
}