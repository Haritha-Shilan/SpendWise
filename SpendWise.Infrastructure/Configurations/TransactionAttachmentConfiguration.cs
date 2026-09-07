namespace SpendWise.Infrastructure.Configurations
{
    public class TransactionAttachmentConfiguration : IEntityTypeConfiguration<TransactionAttachment>
    {
        public void Configure(EntityTypeBuilder<TransactionAttachment> builder)
        {
            builder.ToTable("TransactionAttachment");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TransactionId)
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.FileName)
                .HasColumnType("nvarchar(255)")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.FilePath)
                .HasColumnType("nvarchar(500)")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.ContentType)
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.UploadedAt)
                .HasColumnType("datetime2")
                .IsRequired();

            builder.HasOne(x=>x.Transaction)
                .WithOne(x => x.TransactionAttachment)
                .HasForeignKey<TransactionAttachment>(x => x.TransactionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.TransactionId)
                .IsUnique();
        }
    }
}
