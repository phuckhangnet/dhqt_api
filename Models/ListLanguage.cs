using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class ListLanguage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [System.ComponentModel.DataAnnotations.Key]
        public string? CODE { get; set; }
        public string? LANG { get; set; }
        public string? TEXT { get; set; }
    }
}