namespace Ordering.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(c => c.Id).HasConversion(
                                                  id => id!.Value,
                                                  dbId => OrderId.Of(dbId));
        
        builder.HasOne<Customer>()
               .WithMany()
               .HasForeignKey(o => o.CustomerId)
               .IsRequired();

        builder.HasMany<OrderItem>(o => o.OrderItems)
               .WithOne()
               .HasForeignKey(oi => oi.OrderId);
        
        builder.ComplexProperty(o => o.OrderName,
                                nameBuilder => nameBuilder.Property(n => n.Value)
                                                          .HasColumnName(nameof(Order.OrderName))
                                                          .HasMaxLength(100)
                                                          .IsRequired());
        
        builder.ComplexProperty(o => o.ShippingAddress,
                                nameBuilder =>
                                {
                                    nameBuilder.Property(n => n.FirstName)
                                               .HasMaxLength(50)
                                               .IsRequired();
                                    
                                    nameBuilder.Property(n => n.LastName)
                                               .HasMaxLength(50)
                                               .IsRequired();
                                    
                                    nameBuilder.Property(n => n.EmailAddress)
                                               .HasMaxLength(50)
                                               .IsRequired();
                                    
                                    nameBuilder.Property(n => n.AddressLine)
                                               .HasMaxLength(180)
                                               .IsRequired();
                                    
                                    nameBuilder.Property(n => n.Country)
                                               .HasMaxLength(50)
                                               .IsRequired();
                                    
                                    nameBuilder.Property(n => n.State)
                                               .HasMaxLength(50)
                                               .IsRequired();
                                    
                                    nameBuilder.Property(n => n.ZipCode)
                                               .HasMaxLength(50)
                                               .IsRequired();
                                });
        
        builder.ComplexProperty(o => o.BillingAddress,
                                nameBuilder =>
                                {
                                    nameBuilder.Property(n => n.FirstName)
                                               .HasMaxLength(50)
                                               .IsRequired();
                                    
                                    nameBuilder.Property(n => n.LastName)
                                               .HasMaxLength(50)
                                               .IsRequired();
                                    
                                    nameBuilder.Property(n => n.EmailAddress)
                                               .HasMaxLength(50)
                                               .IsRequired();
                                    
                                    nameBuilder.Property(n => n.AddressLine)
                                               .HasMaxLength(180)
                                               .IsRequired();
                                    
                                    nameBuilder.Property(n => n.Country)
                                               .HasMaxLength(50)
                                               .IsRequired();
                                    
                                    nameBuilder.Property(n => n.State)
                                               .HasMaxLength(50)
                                               .IsRequired();
                                    
                                    nameBuilder.Property(n => n.ZipCode)
                                               .HasMaxLength(50)
                                               .IsRequired();
                                });
        
        builder.ComplexProperty(o => o.Payment,
                                nameBuilder =>
                                {
                                    nameBuilder.Property(n => n.CardName)
                                               .HasMaxLength(50)
                                               .IsRequired();
                                    
                                    nameBuilder.Property(n => n.CardNumber)
                                               .HasMaxLength(24)
                                               .IsRequired();
                                    
                                    nameBuilder.Property(n => n.Expiration)
                                               .HasMaxLength(10)
                                               .IsRequired();
                                    
                                    nameBuilder.Property(n => n.CVV)
                                               .HasMaxLength(3)
                                               .IsRequired();
                                    
                                    nameBuilder.Property(n => n.PaymentMethod);
                                });
        
        builder.Property(o => o.Status)
               .HasDefaultValue(OrderStatus.Draft)
               .HasConversion(s => s.ToString(), s => Enum.Parse<OrderStatus>(s));
        
        builder.Property(o => o.TotalPrice).IsRequired(); 
    }
}