namespace OnlineJudger.Application.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public int Points { get; set; }
        public int PlaceInTop { get; set; }
    }
}
