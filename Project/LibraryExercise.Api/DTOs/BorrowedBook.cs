namespace  LibraryExercise.Api.DTOs
{
    public class BorrowedBook
    {
        public int BorrowId {get; set;}
        public string BookTitle { get; set; } = "";
        public string BookAuthor { get; set; } = "";
        public DateTime BorrowDate { get; set; }
    }
}