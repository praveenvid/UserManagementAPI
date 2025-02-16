using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [Route("api/v{version:apiVersion}/users")]
    //[Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _context;

        public UserController(UserDbContext context)
        {
            _context = context;
        }

        [ApiVersion("1.0")]
        [HttpGet("users")]
        public IActionResult GetUser([FromQuery] int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

        // GET: api/users
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        //{
        //    return await _context.Users.ToListAsync();
        //}

        // GET: api/users/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<User>> GetUser(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null) return NotFound();
        //    return user;
        //}

        // GET api/users?userId=1&username=johndoe
        [HttpGet]
        public async Task<IActionResult> GetUserByQuery([FromQuery] int? userId, [FromQuery] string? username)
        {
            if (userId == null && string.IsNullOrEmpty(username))
            {
                return BadRequest("Please provide at least one parameter (userId or username).");
            }

            var query = _context.Users.AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(u => u.Id == userId.Value);
            }

            if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(u => u.LoginName == username);
            }

            var users = await query.ToListAsync();

            if (users.Count == 0)
            {
                return NotFound("No users found.");
            }

            return Ok(users);
        }

        // POST: api/users
        //[HttpPost]
        //public async Task<ActionResult<User>> PostUser(User user)
        //{
        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();
        //    return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        //}



        //// PUT: api/users/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser(int id, User user)
        //{
        //    if (id != user.Id) return BadRequest();
        //    _context.Entry(user).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
        //    return NoContent();
        //}

        // UPDATE User (PUT api/users/{id})
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            if (id != updatedUser.Id)
            {
                return BadRequest("User ID mismatch.");
            }

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }

            // Update user properties
            existingUser.LoginName = updatedUser.LoginName;
            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.Address = updatedUser.Address;
            existingUser.UserStatus = updatedUser.UserStatus;

            await _context.SaveChangesAsync();

            return Ok(existingUser);
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
