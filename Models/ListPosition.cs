using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class ListPosition
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [System.ComponentModel.DataAnnotations.Key]
        public string? CODE { get; set; }
        public string? DESCRIPTION { get; set; }
    }
}