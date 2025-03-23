using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        var transactions = new[]
        {
            new { Id = 1, Amount = 1000, Type = "Доход", Category = "Зарплата", Date = DateTime.Now },
            new { Id = 2, Amount = -500, Type = "Расход", Category = "Еда", Date = DateTime.Now }
        };

        return Ok(transactions);
    }
}