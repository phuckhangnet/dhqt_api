using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Blogs
    {
        public int ID { get; set; }
        public string? TITLE { get; set; }
        public string? ARTICLECONTENT { get; set; }
        public string? HASTAG { get; set; }
        //public string? AVATAR { get; set; }
        [DataType(DataType.Date)]
        public DateTime CREATEDATE { get; set; }
        [DataType(DataType.Date)]
        public DateTime LATESTEDITDATE { get; set; }
        public int IDUSER { get; set; }
        public string? LANGUAGE { get; set; }
        public Int16? LIKES { get; set; }
        public string? STATUS { get; set; }
    }
}