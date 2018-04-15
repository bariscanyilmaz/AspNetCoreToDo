using System.Collections.Generic;

namespace AspNetCoreToDo.Models
{
    public class TodoViewModel
    {
        public IEnumerable<TodoItem> Items {get;set;}

        
    }
}