using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.API.Models;

public class Transaction
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty;
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}