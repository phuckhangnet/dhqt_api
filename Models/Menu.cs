namespace Project.Models
{
    public class Menu
    {
        public int ID { get; set; }
        public string? NAME { get; set; }
        public string? DESCRIPTION { get; set; }
        public int MENULEVEL { get; set; }
        public string? PARENT { get; set; }
        public Int16 POSITION { get; set; }
        public Byte ISACTIVE { get; set; }
        public Byte ISPAGE { get; set; }
        public Byte VISIBLE { get; set; }
    }
}