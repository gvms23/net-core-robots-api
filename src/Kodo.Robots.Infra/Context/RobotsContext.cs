using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kodo.Robots.Domain.Entities;

namespace Kodo.Robots.Infra.Data.Context
{
    public class RobotsContext : DbContext
    {
        public static readonly LoggerFactory _debugLoggerFactory = new LoggerFactory(new[] { new DebugLoggerProvider() });

        public RobotsContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Robot> Robots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Entities Config (identificação automática)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RobotsContext).Assembly);

            //Configurações globais
            modelBuilder.ApplyGlobalStandards();

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(config.GetConnectionString(nameof(RobotsContext)))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }

            optionsBuilder.EnableSensitiveDataLogging();

            if (Debugger.IsAttached)
                optionsBuilder.UseLoggerFactory(_debugLoggerFactory);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            ChangeTracker.Entries().ToList().ForEach(entry =>
            {
                if (entry.Entity is EntityBase trackableEntity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            trackableEntity.CreatedDate = DateTime.Now;
                            trackableEntity.IsDeleted = false;
                            break;

                        case EntityState.Modified:
                            trackableEntity.ModifiedDate = DateTime.Now;
                            break;
                    }
                }
            });
        }
    }
}
