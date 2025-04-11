namespace AucX.Domain.Entities
{
    public class BannedUser
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public required string Reason { get; set; }
        public DateTime BannedDate { get; set; } = DateTime.UtcNow;
    }
}