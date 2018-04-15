using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreToDo.Models;
using AspNetCoreToDo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreToDo.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;
        private readonly UserManager<ApplicationUser> _userManger;

        public TodoController(ITodoItemService todoItemService,UserManager<ApplicationUser> userManager)
        {
            _todoItemService=todoItemService;
            _userManger=userManager;
        }
        
        public async Task<IActionResult> Index()
        {
            var currenUser=await _userManger.GetUserAsync(User);
            if(currenUser==null) return Challenge();

            var todoItems=await _todoItemService.GetInCompleteItemAsync(currenUser);

            var model=new TodoViewModel()
            {
                Items=todoItems
            };

            return View(model);
        }

        public async Task<IActionResult> AddItem(NewTodoItem newTodoItem)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currenUser=await _userManger.GetUserAsync(User);
            if(currenUser==null) return Unauthorized();

            var successful=await _todoItemService.AddItemAsync(newTodoItem,currenUser);

            if(!successful)
            {
                return BadRequest(new {error="Could not add item "});
            }

            return Ok();
        }

        public async Task<IActionResult> MarkDone(Guid id)
        {
            if(id==Guid.Empty) return BadRequest();

            var currenUser=await _userManger.GetUserAsync(User);
            if(currenUser==null) return Unauthorized();

            var successful=await _todoItemService.MarkDoneAsync(id,currenUser);
            if(!successful) return BadRequest();

            return Ok();
        }

    }
}
