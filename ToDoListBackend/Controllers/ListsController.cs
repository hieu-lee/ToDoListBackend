using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListBackend.Data;
using ToDoListBackend.Models;

namespace ToDoListBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListsController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        public ListsController(AppDbContext context)
        {
            dbContext = context;
        }

        [HttpGet("lists/{username}")]
        public async Task<IActionResult> GetListsAsync(string username)
        {
            var myacc = await dbContext.Accounts.FindAsync(username);
            if (myacc is null)
            {
                return NotFound("Username not found");
            }
            var listsArray = dbContext.Lists.Where(s => s.owner == myacc).ToArray();
            return Ok(listsArray);
        }

        [HttpGet("today-items/{username}")]
        public async Task<IActionResult> GetTodayItemsAsync(string username)
        {
            var acc = await dbContext.Accounts.FindAsync(username);
            if (acc is null)
            {
                return NotFound("Username does not exist");
            }
            var items = dbContext.Items.Where(s => (s.timeRemind == null || s.timeRemind.Value.Date == DateTime.Now.Date) && s.owner == username).ToArray();
            return Ok(items);
        }

        [HttpGet("date-items/{username}/{date}")]
        public async Task<IActionResult> GetItemsFromDateAsync(string username, string date)
        {
            var Date = DateTime.Parse(date).Date;
            var acc = await dbContext.Accounts.FindAsync(username);
            if (acc is null)
            {
                return NotFound("Username does not exist");
            }
            var items = dbContext.Items.Where(s => (s.timeRemind == null || s.timeRemind.Value.Date == Date) && s.owner == username).ToArray();
            return Ok(items);
        }

        [HttpGet("items/{username}/{listid}")]
        public async Task<IActionResult> GetItemsAsync(string username, string listid)
        {
            var res = await dbContext.Lists.FindAsync(listid);
            if (res is null)
            {
                return NotFound("List not found");
            }
            if (res.ownerUsername == username)
            {
                var items = dbContext.Items.Where(s => s.parentListId == listid).ToArray();
                return Ok(items);
            }
            return StatusCode(403, "You don't have access to this list");
        }

        [HttpPost("new-list/{username}")]
        public async Task<IActionResult> CreateNewListAsync(string username,[FromBody] ToDoList list)
        {
            var myacc = dbContext.Accounts.Find(username);
            list.owner = myacc;
            myacc.lists.Add(list);
            dbContext.Lists.Add(list);
            dbContext.Accounts.Update(myacc);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("update-list/{username}")]
        public async Task<IActionResult> UpdateListAsync(string username,[FromBody] ToDoList list)
        {
            var oldlist = dbContext.Lists.Find(list.listId);
            if (oldlist is null)
            {
                return NotFound("List not found");
            }
            if (oldlist.ownerUsername == username)
            {
                var deleteItems = dbContext.Items.Where(s => s.parentListId == list.listId).ToArray();
                dbContext.Items.RemoveRange(deleteItems);
                await dbContext.Items.AddRangeAsync(list.items);
                await dbContext.SaveChangesAsync();
                return Ok();
            }
            return StatusCode(403, "You don't have access to this list");
        }

        [HttpPut("update-today")]
        public async Task<IActionResult> UpdateTodayItemsAsync([FromBody] HashSet<ToDoItem> items)
        {
            dbContext.Items.UpdateRange(items);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("delete-list/{username}/{listid}")]
        public async Task<IActionResult> DeleteListAsync(string username, string listid)
        {
            var list = dbContext.Lists.Find(listid);
            if (list is null)
            {
                return NotFound("List not found");
            }
            if (list.ownerUsername == username)
            {
                dbContext.Items.RemoveRange(list.items);
                var task = dbContext.Accounts.FindAsync(username);
                dbContext.Lists.Remove(list);
                var acc = await task;
                acc.lists.Remove(list);
                dbContext.Accounts.Update(acc);
                await dbContext.SaveChangesAsync();
                return Ok();
            }
            return StatusCode(403, "You don't have access to this list");
        }

        [HttpDelete("delete-item/{username}/{itemid}")]
        public async Task<IActionResult> DeleteItemAsync(string username, string itemid)
        {
            var item = dbContext.Items.Find(itemid);
            if (item is null)
            {
                return NotFound("Item not found");
            }
            if (item.owner != username)
            {
                return StatusCode(403, "You don't have access to this item");
            }
            dbContext.Items.Remove(item);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
