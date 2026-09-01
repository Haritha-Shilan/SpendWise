namespace SpendWise.Infrastructure.Configurations
{
    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.ToTable("PaymentMethod");

            builder.HasKey(x => x.Id);

            builder.Property(x=>x.Name)
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasColumnType("bit")
                .IsRequired();

            builder.HasIndex(x => x.Name)
                .IsUnique();

            //Seed Data
            builder.HasData(
                new PaymentMethod
                {
                    Id = 1,
                    Name = "Cash",
                    IsActive = true
                },
                new PaymentMethod
                {
                    Id = 2,
                    Name = "Credit Card",
                    IsActive = true
                },
                new PaymentMethod
                {
                    Id = 3,
                    Name = "Debit Card",
                    IsActive = true
                },
                new PaymentMethod
                {
                    Id = 4,
                    Name = "UPI",
                    IsActive = true
                },
                new PaymentMethod
                {
                    Id = 5,
                    Name = "Bank Transfer",
                    IsActive = true
                }
            );
        }
    }
}
