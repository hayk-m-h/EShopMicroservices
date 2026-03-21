namespace Ordering.Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(c => c.Id).HasConversion(
                                                  id => id!.Value,
                                                  dbId => OrderItemId.Of(dbId));
        
    }
}