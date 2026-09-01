namespace SpendWise.Infrastructure.Configurations
{
    public class CategoryTypeMasterConfiguration : IEntityTypeConfiguration<CategoryTypeMaster>
    {
        public void Configure(EntityTypeBuilder<CategoryTypeMaster> builder)
        {
            builder.ToTable("CategoryTypeMaster");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasColumnType("nvarchar(20)")
                .HasMaxLength(20)
                .IsRequired();

            //Data Seeding
            builder.HasData(
                new CategoryTypeMaster 
                {
                 Id = 1,
                 Name="Expense"
                },
                new CategoryTypeMaster
                {
                    Id=2,
                    Name = "Income"
                }
                );
        }
    }
}
