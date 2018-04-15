using System.ComponentModel.DataAnnotations;

namespace AspNetCoreToDo.Models
{
    public class NewTodoItem
    {
        [Required]
        public string Title { get; set; }
    }
}