using FinanceTracker.API.DTOs;

namespace FinanceTracker.API.Services;

public interface ITransactionService
{
    Task<List<TransactionDto>> GetAllAsync();
    Task<TransactionDto?> GetByIdAsync(int id);
    Task<TransactionDto> CreateAsync(CreateTransactionDto dto);
    Task<bool> UpdateAsync(int id, UpdateTransactionDto dto);
    Task<bool> DeleteAsync(int id);
}