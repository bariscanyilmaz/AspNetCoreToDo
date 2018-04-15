using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreToDo.Data;
using AspNetCoreToDo.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreToDo.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _context;

        public TodoItemService(ApplicationDbContext context)
        {
            _context=context;
        }

        public async Task<bool> AddItemAsync(NewTodoItem newTodoItem,ApplicationUser user)
        {
            var entity=new TodoItem
            {
                Id=Guid.NewGuid(),
                OwnerId=user.Id,
                isDone=false,
                Title=newTodoItem.Title,
                DueAt=DateTimeOffset.Now.AddDays(3)

            };

            _context.Items.Add(entity);
            var saveResult=await _context.SaveChangesAsync();
            return saveResult==1;
        }


        public async Task<IEnumerable<TodoItem>> GetInCompleteItemAsync(ApplicationUser user)
        {
           var items=await _context.Items.Where(x=>x.isDone==false && x.OwnerId==user.Id).ToArrayAsync();

           return items;
        }

        public async Task<bool> MarkDoneAsync(Guid id,ApplicationUser user)
        {
            var item= await _context.Items.Where(x=>x.Id==id && x.OwnerId==user.Id).SingleOrDefaultAsync();
            if(item==null) return false;

            item.isDone=true;
            var saveResult=await _context.SaveChangesAsync();
            return saveResult==1;
        }


    }
}