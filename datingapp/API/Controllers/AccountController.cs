using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseAppController
    {
        private readonly DataContext _context;


        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDtos>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username))
            {
                return BadRequest("UserName is Taken!!");
            }
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                Username = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDtos{
                Username=user.Username,
                Token=_tokenService.CreateToken(user)
            };
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDtos>> login(LoginDto loginDto)
        {
            var user = await _context.Users.
            Include(p=>p.Photos).SingleOrDefaultAsync(x => x.Username.ToLower() == loginDto.Username.ToLower());
            if (user == null) return Unauthorized("Invalid username");
            var hmac = new HMACSHA512(user.PasswordSalt);
            var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < ComputedHash.Length; i++)
            {
                if (ComputedHash[i] != user.PasswordHash[i]) return Unauthorized("invalid password");
            }
            return new UserDtos{
                   Username=user.Username,
                Token=_tokenService.CreateToken(user),
                PhotoUrl=user.Photos.FirstOrDefault(x=>x.IsMain)?.Url
            };

        }
        private async Task<bool> UserExists(string Username)
        {
            return await _context.Users.AnyAsync(x => x.Username == Username.ToLower());
        }
    }
}