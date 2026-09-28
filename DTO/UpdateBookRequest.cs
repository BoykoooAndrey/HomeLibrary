using System.ComponentModel.DataAnnotations;

namespace HomeLibrary.DTO
{
    public class UpdateBookRequest
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; } = null!;

        [Range(1, 9999)]
        public int PublicationYear { get; set; }

        public string? Content { get; set; }

        [MinLength(1, ErrorMessage = "Необходимо выбрать хотя бы одного автора.")]
        public List<int> AuthorIds { get; set; } = [];
    }
}