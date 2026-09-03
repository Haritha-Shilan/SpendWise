using SpendWise.API.Features.Notification;

namespace SpendWise.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //DbContext
            builder.Services.AddDbContext<SpendWiseDbContext>(options=>
            options.UseSqlServer(builder.Configuration.GetConnectionString("SpendWiseConnection")));

            //IdentityContext
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<SpendWiseDbContext>()
                .AddDefaultTokenProviders();

            //Generic Repository
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            //Mapper
            builder.Services.AddAutoMapper(cfg =>
            { 
            }, typeof(MappingProfile));

            //Add Feature Services
            builder.Services.AddScoped<ICategoryMasterService, CategoryMasterService>();
            builder.Services.AddScoped<IPaymentMethodService,PaymentMethodService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();

            var app = builder.Build();

            //Invoke runtime seeder
            using (var scope= app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                await IdentitySeeder.SeedAsync(userManager, roleManager,builder.Configuration);

            }

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            await app.RunAsync();
        }
    }
}
