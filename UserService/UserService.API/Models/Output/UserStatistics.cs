namespace UserService.API.Models.Output
{
    public class UserStatistics
    {
        public Guid UserId { get; set; }
        public int AmountOfItems { get; set; }
        public int AmountOfLendings { get; set; }
        public int AmountOfBorrowings { get; set; }
        public int TotalScore { get; set; }
    }
}
