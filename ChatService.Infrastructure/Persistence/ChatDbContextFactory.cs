using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;

namespace ChatService.Infrastructure.Persistence
{
    public class ChatDbContextFactory
    {
        public class AppDbContextFactory : IDesignTimeDbContextFactory<ChatDbContext>
        {
            public ChatDbContext CreateDbContext(string[] args)
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

                var optionsBuilder = new DbContextOptionsBuilder<ChatDbContext>();

                optionsBuilder.UseNpgsql(
                    config.GetConnectionString("AppDb"));

                return new ChatDbContext(optionsBuilder.Options);
            }
        }
    }
}
