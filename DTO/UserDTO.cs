namespace ExpenseTracker.DTO
{
    public class UserDTO
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<int>? ExpenseIds { get; set; }
    }
}
