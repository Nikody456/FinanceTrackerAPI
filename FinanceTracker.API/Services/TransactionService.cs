using FinanceTracker.API.Data;
using FinanceTracker.API.DTOs;
using FinanceTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Services;

public class TransactionService(FinanceTrackerDbContext context) : ITransactionService
{
    public async Task<List<TransactionDto>> GetAllAsync()
    {
        return await context.Transactions
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Type = t.Type,
                Category = t.Category,
                Date = t.Date
            }).ToListAsync();
    }

    public async Task<TransactionDto?> GetByIdAsync(int id)
    {
        var t = await context.Transactions.FindAsync(id);
        if (t == null) return null;

        return new TransactionDto
        {
            Id = t.Id,
            Amount = t.Amount,
            Type = t.Type,
            Category = t.Category,
            Date = t.Date
        };
    }

    public async Task<TransactionDto> CreateAsync(CreateTransactionDto dto)
    {
        var transaction = new Transaction
        {
            Amount = dto.Amount,
            Type = dto.Type,
            Category = dto.Category,
            Date = dto.Date
        };

        context.Transactions.Add(transaction);
        await context.SaveChangesAsync();

        return new TransactionDto
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Type = transaction.Type,
            Category = transaction.Category,
            Date = transaction.Date
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateTransactionDto dto)
    {
        var transaction = await context.Transactions.FindAsync(id);
        if (transaction == null) return false;

        transaction.Amount = dto.Amount;
        transaction.Type = dto.Type;
        transaction.Category = dto.Category;
        transaction.Date = dto.Date;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var transaction = await context.Transactions.FindAsync(id);
        if (transaction == null) return false;

        context.Transactions.Remove(transaction);
        await context.SaveChangesAsync();
        return true;
    }
}
