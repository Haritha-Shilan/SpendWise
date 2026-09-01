namespace SpendWise.Infrastructure.Data
{
    public class SpendWiseDbContext :IdentityDbContext<ApplicationUser>
    {
        public SpendWiseDbContext(DbContextOptions<SpendWiseDbContext> options)
            :base(options)
        {      
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(SpendWiseDbContext).Assembly);
        }

        public DbSet<CategoryTypeMaster> CategoryTypeMasters { get; set; }

        public DbSet<CategoryMaster> CategoryMasterMasters { get; set; }

        public DbSet<UserCategory> UserCategories { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<TransactionAttachment> TransactionAttachments { get; set; }

        public DbSet<Notification> Notifications { get; set; }
    }
}
