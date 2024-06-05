using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksApi.Models
{
    public class Book
    {
        public string Isbn { get; set; } = "";
        [Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; } = "";
        [Column(TypeName = "nvarchar(100)")]
        public string Language { get; set; } = ""; // Extracted from title attribute
        [Column(TypeName = "nvarchar(16)")]
        public List<string> Authors { get; set; } = [];
        public int Year { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public decimal Price { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Category { get; set; } = "";
        [Column(TypeName = "nvarchar(100)")]
        public string Cover { get; set; } = "";
        
    }
}
