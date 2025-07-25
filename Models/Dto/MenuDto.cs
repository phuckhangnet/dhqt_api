namespace Project.Models.Dto
{
    public class MenuDto
    {
        public int ID { get; set; }
        public string? NAME { get; set; }
        public string? DESCRIPTION { get; set; }
        public int MENULEVEL { get; set; }
        public string? PARENT { get; set; }
        public Int16 POSITION { get; set; }
        public Byte IsActive { get; set; }
        public Byte IsPage { get; set; }
        public Byte VISIBLE { get; set; }
    }
}