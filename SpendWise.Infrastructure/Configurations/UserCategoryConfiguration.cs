namespace SpendWise.Infrastructure.Configurations
{
    public class UserCategoryConfiguration : IEntityTypeConfiguration<UserCategory>
    {
        public void Configure(EntityTypeBuilder<UserCategory> builder)
        {
            builder.ToTable("UserCategory");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnType("nvarchar(450)")
                .IsRequired();


            builder.Property(x => x.TypeId)
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasColumnType("bit")
                .IsRequired();

            builder.HasOne(x => x.ApplicationUser)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(X=>X.CategoryTypeMaster)
                .WithMany()
                .HasForeignKey(x=>x.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new
            {
                x.UserId,
                x.TypeId,
                x.Name
            })
                .IsUnique();

        }
    }
}
