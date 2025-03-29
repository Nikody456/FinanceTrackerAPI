namespace FinanceTracker.API.DTOs;

public class CreateTransactionDto
{
    public decimal Amount { get; set; }
    public string Type { get; set; } = null!;
    public string Category { get; set; } = null!;
    public DateTime Date { get; set; }
}