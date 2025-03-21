namespace ExpenseTracker.DTO
{
    public class ExpenseDTO
    {
        public int Id { get; set; }
        public required string Category { get; set; }
        public required string Description { get; set; }
        public required double Amount { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }

    }
}
