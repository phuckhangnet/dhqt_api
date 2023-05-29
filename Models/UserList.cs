using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class UserList
    {
        public int USERID { get; set; }
        public string? TABLELIST { get; set; }
        public string? LISTCODE { get; set; }
    }
}