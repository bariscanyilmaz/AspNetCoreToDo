using System.Collections.Generic;

namespace AspNetCoreToDo.Models
{
    public class ManageUsersViewModel
    {
        public IEnumerable<ApplicationUser> Administrators { get; set; }
        public IEnumerable<ApplicationUser> Everyone { get; set; }

    }
}