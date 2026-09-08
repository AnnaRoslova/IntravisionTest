namespace IntravisionTest.Models
{
    public class Drink
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public required string ImageUrl { get; set; }
    }
}
