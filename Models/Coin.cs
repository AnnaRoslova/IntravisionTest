namespace IntravisionTest.Models
{
    public class Coin
    {
        public Guid Id { get; set; }
        public int Denomination { get; set; }
        public int Count { get; set; }
        public bool IsBlocked { get; set; }
    }
}
