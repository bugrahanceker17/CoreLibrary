using CoreLibrary.Models.Concrete.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Utilities.DataAccess;

public class ApplicationDbContext : DbContext
{
    private readonly string _connectionString;
    public ApplicationDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging(true);
        optionsBuilder.UseSqlServer(_connectionString);
    }

    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<AppRole> AppRoles { get; set; }
    public DbSet<AppUserRole> AppUserRoles { get; set; }
    public DbSet<AppPermission> Permissions { get; set; }
    public DbSet<AppPermissionCorrelation> AppPermissionCorrelations { get; set; }
    public DbSet<AppLoginLog> AppLoginLogs { get; set; }
}