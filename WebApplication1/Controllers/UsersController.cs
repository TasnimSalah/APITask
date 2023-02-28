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
using WebApplication1.Services;
using System.IdentityModel.Tokens.Jwt;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        public UsersController(AppDbContext context , IAuthService authService)
        {
            _context = context;
            this._authService = authService;
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

            if (UserExists(model.Email))
            {
                return Conflict(new { ErrorMessage = "User Already Exist" });
            }

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
                var token = await _authService.CreateJwtToken(User);
                var result = new AddUserResponseVm()
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    Id = User.Id
                };
                return Ok(result);
            }
            catch (Exception exc)
            {
                return Conflict();
            }       
        }


        private bool UserExists(string email)
        {
            return _context.Users.Any(e => e.Email == email);
        }
    }
}
