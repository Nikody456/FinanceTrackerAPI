using Microsoft.EntityFrameworkCore;
using FinanceTracker.API.Models;

namespace FinanceTracker.API.Data
{
    public class FinanceTrackerDbContext(DbContextOptions<FinanceTrackerDbContext> options) : DbContext(options)
    {
        public DbSet<Transaction> Transactions { get; set; }
    }
}