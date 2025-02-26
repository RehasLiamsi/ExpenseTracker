﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using System.ComponentModel.DataAnnotations;
using ExpenseTracker.DTO;

namespace ExpenseTracker.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _context.Users
        .Include(u => u.Expenses)
        .Select(u => new UserDTO
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            ExpenseIds = u.Expenses.Select(e => e.Id).ToList()
        })
        .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetOneUser(int id)
        {
            var user = await _context.Users
        .Include(u => u.Expenses) 
        .Where(u => u.Id == id)
        .Select(u => new UserDTO
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            ExpenseIds = u.Expenses.Select(e => e.Id).ToList()
        })
        .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser(UserDTO userDTO)
        {
            var user = new User
            {
                Username = userDTO.Username,
                Email = userDTO.Email,
                PasswordHash = "hashed_password_here", 
                Expenses = new List<Expense>() 
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var createdUserDTO = new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                ExpenseIds = user.Expenses.Select(e => e.Id).ToList()
            };

            return CreatedAtAction(nameof(GetOneUser), new { id = user.Id }, createdUserDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User updatedUser)
        {
            if (id != updatedUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(updatedUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(u => u.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

