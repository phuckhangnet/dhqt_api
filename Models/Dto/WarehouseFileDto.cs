namespace Project.Models.Dto
{
    public class WarehouseFileDto
    {
        public int ID { get; set; }
        public string? FILENAME { get; set; }
        public int? FILESIZE { get; set; }
        public string? FILEEXTENSION { get; set; }
        public int IDUSER { get; set; }

    }
}