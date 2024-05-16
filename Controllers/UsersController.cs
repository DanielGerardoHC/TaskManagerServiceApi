using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskManagerServiceApi.Context;
using TaskManagerServiceApi.RequestModel;

namespace TaskManagerServiceApi.Controllers
{
    // abreviatura para Task Manager "TM"
    [Route("TM/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TaskManagerDbContext _context;

        public UsersController(TaskManagerDbContext context)
        {
            _context = context;
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Authenticate([FromBody] User model)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName 
            && x.PasswordHash == model.PasswordHash);

            if (user == null)
            {
                return Unauthorized();
            }
            var token = GenerateJwtToken(user);

            return Ok(new { Token = token });
        }

        
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<User>> GetUser()
        {
            var userID = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (userID == null)
            {
                return Unauthorized();
            }
            User? user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == int.Parse(userID));
            if (user != null)
            {
                user.PasswordHash = null;
                return user;
            }

            return NotFound();
        }

        [Authorize] 
        [HttpPut("ChangePass")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest? passwordRequest)
        {
            if (passwordRequest != null)
            {
                User? userToChangePass = await _context.Users.FirstOrDefaultAsync(e =>  e.PasswordHash == passwordRequest.OldPassword && 
                    e.UserName == passwordRequest.UserName); 
                
                if(userToChangePass != null)
                {
                     userToChangePass.PasswordHash = passwordRequest.NewPassword;
                    _context.Entry(userToChangePass).State = EntityState.Modified;
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return Conflict();
                    }
                }
                else
                {
                    NotFound();
                }
            }
            else
            {
                BadRequest();
            }

            return NoContent();
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(User user, int id)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUser", new { id = user.UserId }, user);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
        
        [Authorize]
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
        
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
        
        private string GenerateJwtToken(User user)
        {
            // crear la configuración del token (key, tiempo de expiración, etc.)
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("i1qP^mj86%6JdnpVHrB&i#0vsA!eL^oW");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                      new Claim("userName", user.UserName),
                      new Claim("email", user.Email),
                      new Claim("fullName", user.FullName),
                      new Claim("userId", user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(2), // tiempo de expiración del token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                            SecurityAlgorithms.HmacSha256Signature),
                Issuer = "TaskManagerServiceApi",
                Audience = "TaskManager"
            };
            // generar el token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
