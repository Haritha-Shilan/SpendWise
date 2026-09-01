namespace SpendWise.Infrastructure.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transaction");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.UserId)
                .HasColumnType("nvarchar(450)")
                .IsRequired();

            builder.Property(x => x.UserCategoryId)
                .HasColumnType("int")
                .IsRequired();

            builder.Property(X => X.PaymentMethodId)
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.Date)
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnType("nvarchar(500)")
                .HasMaxLength(500)
                .IsRequired();

            builder.HasOne(x => x.ApplicationUser)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UserCategory)
                .WithMany()
                .HasForeignKey(x => x.UserCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PaymentMethod)
                .WithMany()
                .HasForeignKey(x => x.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
