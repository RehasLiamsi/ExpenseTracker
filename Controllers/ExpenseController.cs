using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.DTO;
using Microsoft.AspNetCore.Authorization;


namespace ExpenseTracker.Controllers
{
    [Route("api/expenses")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExpenseController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDTO>>> GetExpenses()
        {
            var expenses = await _context.Expenses
                .Select(e => new ExpenseDTO
                {
                    Id = e.Id,
                    Category = e.Category,
                    Description = e.Description,
                    Amount = e.Amount,
                    Date = e.Date,
                    UserId = e.UserId
                })
                .ToListAsync();

            return Ok(expenses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetOneExpense(int id)
        {
            var expense = await _context.Expenses
        .Where(e => e.Id == id)
        .Select(e => new ExpenseDTO
        {
            Id = e.Id,
            Category = e.Category,
            Description = e.Description,
            Amount = e.Amount,
            Date = e.Date,
            UserId = e.UserId
        })
        .FirstOrDefaultAsync();

            if (expense == null)
            {
                return NotFound();
            }

            return Ok(expense);
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseDTO>> CreateExpense(ExpenseDTO expenseDTO)
        {
            var user = await _context.Users.FindAsync(expenseDTO.UserId);
            if (user == null)
            {
                return BadRequest(new { message = "User with the given ID does not exist." });
            }

            var expense = new Expense
            {
                Category = expenseDTO.Category,
                Description = expenseDTO.Description,
                Amount = expenseDTO.Amount,
                Date = DateTime.SpecifyKind(expenseDTO.Date, DateTimeKind.Utc), // Ensure Date is UTC
                UserId = expenseDTO.UserId,
                User = user
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            var createdExpenseDTO = new ExpenseDTO
            {
                Id = expense.Id,
                Category = expense.Category,
                Description = expense.Description,
                Amount = expense.Amount,
                Date = expense.Date,
                UserId = expense.UserId
            };

            return CreatedAtAction(nameof(GetOneExpense), new { id = expense.Id }, createdExpenseDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, Expense updatedExpense)
        {
            if (id!= updatedExpense.Id)
            {
                return BadRequest();
            }

            _context.Entry(updatedExpense).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Expenses.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
