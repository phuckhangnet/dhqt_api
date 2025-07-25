using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class ListTitle
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [System.ComponentModel.DataAnnotations.Key]
        public string? CODE { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? LANGUAGE { get; set; }
    }
}