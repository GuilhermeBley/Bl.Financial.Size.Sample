using Bl.Financial.Size.Sample.Application.Model;
using Bl.Financial.Size.Sample.Application.Repository;
using Microsoft.EntityFrameworkCore;

namespace Bl.Financial.Size.Sample.Server.Repository;

public class SqlFinancialContext : FinancialContext
{
    private readonly string? _connectionString;

    public SqlFinancialContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SqlServer");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            _connectionString,
            config =>
            {
                config.EnableRetryOnFailure(5);
            });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("sizetest");

        modelBuilder.Entity<CompanyModel>(b =>
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("BIGINT")
                .IsRequired();
            b.Property(x => x.ServiceKind)
                .HasConversion<string>()
                .HasColumnType("NVARCHAR(50)")
                .IsRequired();
            b.Property(x => x.Name)
                .HasColumnType("NVARCHAR(500)")
                .IsRequired();
            b.Property(x => x.MonthlyBilling)
                .HasColumnType("DECIMAL(19, 4)")
                .IsRequired();

            b.HasIndex(b => b.Cnpj).IsUnique();
        });

        modelBuilder.Entity<NfModel>(b =>
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("BIGINT")
                .IsRequired();
            b.Property(x => x.CompanyId)
                .HasColumnType("BIGINT")
                .IsRequired();
            b.Property(x => x.DueDate)
                .HasColumnType("DATE")
                .IsRequired();
            b.Property(x => x.Number)
                .HasColumnType("BIGINT")
                .IsRequired();
            b.Property(x => x.UniqueId)
                .HasColumnType("VARCHAR(50)")
                .IsRequired();
            b.Property(x => x.Value)
                .HasColumnType("DECIMAL(19, 4)")
                .IsRequired();

            b.HasIndex(b => b.UniqueId).IsUnique();
        });

        modelBuilder.Entity<NfAnticipationModel>(b =>
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("BIGINT")
                .IsRequired();
            b.Property(x => x.NfId)
                .HasColumnType("BIGINT")
                .IsRequired();
            b.Property(x => x.Desagio)
                .HasColumnType("DECIMAL(19, 4)")
                .IsRequired();
            b.Property(x => x.LiquidValue)
                .HasColumnType("DECIMAL(19, 4)")
                .IsRequired();
            b.Property(x => x.TotalValue)
                .HasColumnType("DECIMAL(19, 4)")
                .IsRequired();
        });
    }
}
