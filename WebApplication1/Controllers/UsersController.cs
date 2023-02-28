using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.ViewModels;
using WebApplication1.Extensions;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(CreateUserVm model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var User = new User()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MarketingConsent = model.MarketingConsent
            };
            User.Id = model.Email.HashSha1();


            try
            {
                _context.Users.Add(User);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exc)
            {
                if (UserExists(User.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            // return CreatedAtAction("GetUser", new { id = user.Id }, user);
            return Ok();
        }


        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
