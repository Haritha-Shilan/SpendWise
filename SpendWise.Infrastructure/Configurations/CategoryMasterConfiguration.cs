namespace SpendWise.Infrastructure.Configurations
{
    public class CategoryMasterConfiguration : IEntityTypeConfiguration<CategoryMaster>
    {
        public void Configure(EntityTypeBuilder<CategoryMaster> builder)
        {
            builder.ToTable("CategoryMaster");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.TypeId)
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasColumnType("bit")
                .IsRequired();

            builder.HasOne(x=>x.CategoryTypeMaster)
                .WithMany()
                .HasForeignKey(x=>x.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new
            {
                x.Name,
                x.TypeId
            })
                .IsUnique();

            //Seed Data
            builder.HasData(
                new CategoryMaster
                {
                    Id = 1,
                    Name = "Food",
                    TypeId = 1,
                    IsActive = true
                },
                new CategoryMaster
                {
                    Id = 2,
                    Name = "Transport",
                    TypeId = 1,
                    IsActive = true
                },
                new CategoryMaster
                {
                    Id = 3,
                    Name = "Shopping",
                    TypeId = 1,
                    IsActive = true
                },
                new CategoryMaster
                {
                    Id = 4,
                    Name = "Bills",
                    TypeId = 1,
                    IsActive = true
                },
                new CategoryMaster
                {
                    Id = 5,
                    Name = "Entertainment",
                    TypeId = 1,
                    IsActive = true
                },
                new CategoryMaster
                {
                    Id = 6,
                    Name = "Healthcare",
                    TypeId = 1,
                    IsActive = true
                },
                new CategoryMaster
                {
                    Id = 7,
                    Name = "Education",
                    TypeId = 1,
                    IsActive = true
                },
                new CategoryMaster
                {
                    Id = 8,
                    Name = "Salary",
                    TypeId = 2,
                    IsActive = true
                },
                new CategoryMaster
                {
                    Id = 9,
                    Name = "Business",
                    TypeId = 2,
                    IsActive = true
                },
                new CategoryMaster
                {
                    Id = 10,
                    Name = "Investment",
                    TypeId = 2,
                    IsActive = true
                },
                new CategoryMaster
                {
                    Id = 11,
                    Name = "Gift",
                    TypeId = 2,
                    IsActive = true
                },
                new CategoryMaster
                {
                    Id = 12,
                    Name = "Other Income",
                    TypeId = 2,
                    IsActive = true
                }
            );
        }
    }
}
