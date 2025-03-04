using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.DTO;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using AutoMapper.QueryableExtensions;


namespace ExpenseTracker.Controllers
{
    [Authorize]
    [Route("api/expenses")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IMapper _mapper;

        public ExpenseController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDTO>>> GetExpenses()
        {
            var expenses = await _context.Expenses
                .ProjectTo<ExpenseDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(expenses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetOneExpense(int id)
        {
            var expense = await _context.Expenses
        .Where(e => e.Id == id)
        .ProjectTo<ExpenseDTO>(_mapper.ConfigurationProvider)
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

            return CreatedAtAction(nameof(GetOneExpense), new { id = expense.Id }, _mapper.Map<ExpenseDTO>(expense));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, ExpenseDTO expenseDTO)
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null || expense.UserId != userId)
            {
                return NotFound(new { message = "Expense not found ot not authorized to update" });
            }

            expense.Category = expenseDTO.Category;
            expense.Description = expenseDTO.Description;
            expense.Amount = expenseDTO.Amount;
            expense.Date = expenseDTO.Date;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "Concurrency error occurred while updating the expense" });
            }

            return Ok(_mapper.Map<ExpenseDTO>(expense));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null || expense.UserId != userId)
            {
                return NotFound(new { message = "Expense not found or not authorized to delete" });
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ExpenseDTO>(expense));
        }
    }
}
