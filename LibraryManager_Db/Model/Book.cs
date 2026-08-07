namespace LibraryManager_Db.Model
{
    public class Book
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        public string Genre { get; set; } = string.Empty;

        [Required]
        public double Price { get; set; } = string.Empty;

        [Required]
        public int Stock { get; set; } = string.Empty;
    }
}