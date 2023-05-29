using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Role_Menu
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [System.ComponentModel.DataAnnotations.Key]
        public string? ROLECODE { get; set; }
        public int MENUID { get; set; }
    }
}