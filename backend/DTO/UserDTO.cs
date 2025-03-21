namespace ExpenseTracker.DTO
{
    public class UserDTO
    {
        public int? Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public List<int>? ExpenseIds { get; set; }
    }
}
