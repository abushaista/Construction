using arif.Construction.Domain.Config;
using arif.Construction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace arif.Construction.Infrastructure;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    private IConfiguration Configuration { get; set; }
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
        Configuration = builder.Build();

        var optionsBuilder =
            new DbContextOptionsBuilder<ApplicationDbContext>();
        var dbOptions = new DbOptions();
        Configuration.Bind(DbOptions.SectionName, dbOptions);
        optionsBuilder.UseNpgsql(connectionString: dbOptions.ConnectionString);
        return new ApplicationDbContext(options: optionsBuilder.Options);
    }
}
