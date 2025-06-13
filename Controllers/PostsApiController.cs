using CatGram.Data;
using CatGram.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatGram.Controllers
{
    [Route("api/posts")]
    [ApiController]
    [Authorize]
    public class PostsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostsApiController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("myposts")]
        public async Task<IActionResult> GetMyPosts()
        {
            var userId = _userManager.GetUserId(User);
            var profile = await _context.CatProfiles
                .Include(c => c.Posts)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);

            if (profile == null)
                return NotFound("No cat profile found.");

            return Ok(profile.Posts);
        }
    }
}