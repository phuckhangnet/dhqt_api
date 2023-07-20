namespace Project.Models
{
    public class User
    {
        public int ID { get; set; }
        public string? USERNAME { get; set; }
        public string? PASSWORD { get; set; }
        public string? PASSWORDHASH { get; set; }
        public string? PASSWORDSALT { get; set; }
        public string? ROLE { get; set; }
        public string? AVATAR { get; set; }
        public int STATUS { get; set; }
        public DateTime CREATEDATE { get; set; }
        public int? COLINDEX { get; set; }

    }
}