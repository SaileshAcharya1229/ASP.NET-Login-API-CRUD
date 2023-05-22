using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ContactsAPIDbContext dbContext;

        public LoginController(ContactsAPIDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }


        [HttpGet]
        public async  Task<IActionResult> CheckUser([FromQuery]RequestUser  User)
        {
            bool isUserExists = await dbContext.tbl_User.AnyAsync(u => u.UserName == User.UserName && u.Password == User.Password);
            if(isUserExists)
            {
                return Ok("User is valid");
            }
            else
            {
                return NotFound();
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> InsertUser(RequestUser requestUser)
        {
            var user = new UserLogin()
            {
                Id = Guid.NewGuid(),
                UserName = requestUser.UserName,
                Password = requestUser.Password
            };


           await dbContext.tbl_User.AddAsync(user);
            await dbContext.SaveChangesAsync();


            return Ok(user);
        }
    }
}
