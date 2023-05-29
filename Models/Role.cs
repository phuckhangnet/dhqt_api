using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Role
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [System.ComponentModel.DataAnnotations.Key]
        public string? CODE { get; set; }
        public string? DESCRIPTION { get; set; }
        public Byte ROLELEVEL { get; set; }
    }
}