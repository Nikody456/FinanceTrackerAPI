using FinanceTracker.API.Data;
using FinanceTracker.API.DTOs;
using FinanceTracker.API.Models;
using FinanceTracker.API.Services;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Tests;

public class TransactionServiceTests
{
    [Fact]
    public async Task CreateTransaction_ShouldReturnTransactionId()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        await using var context = new FinanceTrackerDbContext(options);
        var service = new TransactionService(context);

        var dto = new CreateTransactionDto
        {
            Amount = 100,
            Type = "Доход",
            Category = "Зарплата"
        };

        // Act
        var result = await service.CreateAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Amount, result.Amount);
        Assert.Equal(dto.Type, result.Type);
        Assert.Equal(dto.Category, result.Category);

        Assert.Single(context.Transactions.ToList());
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTransactions()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
            .UseInMemoryDatabase("GetAllDb")
            .Options;

        await using var context = new FinanceTrackerDbContext(options);
        context.Transactions.AddRange(
            new Transaction { Amount = 100, Type = "Доход", Category = "Зарплата", Date = DateTime.Now },
            new Transaction { Amount = -50, Type = "Расход", Category = "Еда", Date = DateTime.Now }
        );
        await context.SaveChangesAsync();

        var service = new TransactionService(context);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnTransaction_WhenExists()
    {
        var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
            .UseInMemoryDatabase("GetByIdDb")
            .Options;

        using var context = new FinanceTrackerDbContext(options);
        var transaction = new Transaction { Amount = 200, Type = "Доход", Category = "Фриланс", Date = DateTime.Now };
        context.Transactions.Add(transaction);
        await context.SaveChangesAsync();

        var service = new TransactionService(context);

        var result = await service.GetByIdAsync(transaction.Id);

        Assert.NotNull(result);
        Assert.Equal(transaction.Amount, result.Amount);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldModifyTransaction_WhenExists()
    {
        var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
            .UseInMemoryDatabase("UpdateDb")
            .Options;

        await using var context = new FinanceTrackerDbContext(options);
        var transaction = new Transaction { Amount = 100, Type = "Доход", Category = "Подарок", Date = DateTime.Now };
        context.Transactions.Add(transaction);
        await context.SaveChangesAsync();

        var service = new TransactionService(context);

        var updateDto = new UpdateTransactionDto
        {
            Amount = 300,
            Type = "Доход",
            Category = "Подарок",
            Date = DateTime.Now
        };

        var result = await service.UpdateAsync(transaction.Id, updateDto);

        Assert.True(result);
        Assert.Equal(300, context.Transactions.First().Amount);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldRemoveTransaction_WhenExists()
    {
        var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
            .UseInMemoryDatabase("DeleteDb")
            .Options;

        await using var context = new FinanceTrackerDbContext(options);
        var transaction = new Transaction { Amount = 500, Type = "Доход", Category = "Бонус", Date = DateTime.Now };
        context.Transactions.Add(transaction);
        await context.SaveChangesAsync();

        var service = new TransactionService(context);

        var result = await service.DeleteAsync(transaction.Id);

        Assert.True(result);
        Assert.Empty(context.Transactions);
    }
}