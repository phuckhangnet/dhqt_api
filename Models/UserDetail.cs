using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class UserDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [System.ComponentModel.DataAnnotations.Key]
        public int USERID { get; set; }
        public string? NAME { get; set; }
        public string? PHONE { get; set; }
        public string? EMAIL { get; set; }
        public string? EDUCATION { get; set; }
        public string? OFFICE { get; set; }
        public string? MAJOR { get; set; }
        public string? RESEARCH { get; set; }
        public string? SUPERVISION { get; set; }
        public string? PUBLICATION { get; set; }
        public string? LANGUAGE { get; set; }
        public string? TEACHINGCOURSE { get; set; }
        public string? ABOUTME { get; set; }
    }
}