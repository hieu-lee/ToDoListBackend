using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ToDoListBackend.Data;
using ToDoListBackend.Models;
using ToDoListBackend.Services;

namespace ToDoListBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly EncryptionAndCompressService encryptionService;

        public AccountsController(AppDbContext context, EncryptionAndCompressService encrypt)
        {
            dbContext = context;
            encryptionService = encrypt;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] Account account)
        {
            var acc = dbContext.Accounts.Find(account.username);
            if (acc is null)
            {
                account.password = encryptionService.Encrypt(account.password);
                dbContext.Accounts.Add(account);
                await dbContext.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return StatusCode(403, "Your username has been taken");
            }
        }

        [HttpPost("sign-in")]
        public IActionResult SignIn([FromBody] Account account)
        {
            var acc = dbContext.Accounts.Find(account.username);
            if (acc is null)
            {
                return NotFound("Your username does not exist");
            }
            if (acc.password != encryptionService.Encrypt(account.password))
            {
                return StatusCode(403, "Your password is incorrect");
            }
            return Ok();
        }
    }
}
