namespace Project.Models
{
    public class Upload_Files_Mart
    {
        public int ID { get; set; }
        public string? FILENAME { get; set; }
        public int? FILESIZE { get; set; }
        public string? FILEEXTENSION { get; set; }
        public int IDUSER { get; set; }

    }
}