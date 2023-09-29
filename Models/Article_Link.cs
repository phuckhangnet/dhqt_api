using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Article_Link
    {
        public int SOURCEARTICLE { get; set; }
        public string? SOURCEARTICLELANG { get; set; }
        public int LINKARTICLE { get; set; }
        public string? LINKARTICLELANG { get; set; }
    }
}