using CoreLibrary.Models.Concrete.Entities;
using CoreLibrary.Models.Concrete.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Utilities.DataAccess;

public class ApplicationDbContext : DbContext
{
    private readonly string _connectionString;
    private readonly string _dbType;
    
    public ApplicationDbContext(string connectionString, string dbType)
    {
        _connectionString = connectionString;
        _dbType = dbType;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_dbType.ToLower() == "sqlserver")
        {
             optionsBuilder.UseSqlServer(_connectionString);
        }
        else if (_dbType.ToLower() == "postgresql")
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }
        else if (_dbType.ToLower() == "mariadb")
        {
            optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
        }
       
        optionsBuilder.LogTo(Console.WriteLine);
        optionsBuilder.EnableSensitiveDataLogging(true);
        optionsBuilder.EnableDetailedErrors();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (_dbType.ToLower() == "postgresql")
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity<Guid>).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity<Guid>.Id))
                        .HasColumnType("uuid");
                
                    modelBuilder.HasDefaultSchema("CoreLibraryTest");
                }
            }
        
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            base.OnModelCreating(modelBuilder);
        }
    }


    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<AppRole> AppRoles { get; set; }
    public DbSet<AppUserRole> AppUserRoles { get; set; }
    public DbSet<AppPermission> Permissions { get; set; }
    public DbSet<AppPermissionCorrelation> AppPermissionCorrelations { get; set; }
    public DbSet<AppLoginLog> AppLoginLogs { get; set; }
    public DbSet<AppLocalizationValue> AppLocalizationValues { get; set; }
}