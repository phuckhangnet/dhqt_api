namespace Project.Models.Dto
{
    public class UserDto
    {
        public int ID { get; set; }
        public string? ROLE { get; set; }
        public string? USERNAME { get; set; }
        public string? AVATAR { get; set; }
        public int STATUS { get; set; }
        public DateTime CREATEDATE { get; set; }
        public int? COLINDEX { get; set; }
        public string? KEY { get; set; }
    }
    public class UserLoginDto : UserDto
    {
        public string? TOKEN { get; set; }
        public string? TOKENALIVETIME { get; set; }
    }
}