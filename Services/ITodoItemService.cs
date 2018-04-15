using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreToDo.Models;

namespace AspNetCoreToDo.Services
{
    public interface ITodoItemService
    {
        Task<IEnumerable<TodoItem>> GetInCompleteItemAsync(ApplicationUser user);

        Task<bool> AddItemAsync(NewTodoItem newTodoItem, ApplicationUser user);

        Task<bool> MarkDoneAsync(Guid id, ApplicationUser user);

        
        
    }

}