namespace LibraryExercise.Api.Models
{
    public class Book
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string Category { get; set; }
        public required int NoOfAvailableCopies { get; set; }
        public string? ImageUrl { get; set; }
    }
}