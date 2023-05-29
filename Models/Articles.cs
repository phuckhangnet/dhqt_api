using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Articles
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string? TITLE { get; set; }
        public string? SUMMARY { get; set; }
        public string? ARTICLECONTENT { get; set; }
        public string? HASTAG { get; set; }
        public string? AVATAR { get; set; }
        public int PRIORITYLEVEL { get; set; }
        public string? LANGUAGE { get; set; }
        [DataType(DataType.Date)]
        public DateTime CREATEDATE { get; set; }
        [DataType(DataType.Date)]
        public DateTime LATESTEDITDATE { get; set; }
        public int IDUSERCREATE { get; set; }
        public int IDUSEREDIT { get; set; }
    }
}