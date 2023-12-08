using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gls_Etykiety.Domain;

public class LabelDbContextFactory : IDesignTimeDbContextFactory<LabelDbContext>
{
    public LabelDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<LabelDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("Db"));

        return new LabelDbContext(optionsBuilder.Options);
    }
}
