using Bl.Financial.Size.Sample.Application.Model;
using Microsoft.EntityFrameworkCore;

namespace Bl.Financial.Size.Sample.Application.Repository;
public abstract class FinancialContext : DbContext
{
    public DbSet<NfModel> Nfs { get; set; }
    public DbSet<CompanyModel> Companies { get; set; }

}
