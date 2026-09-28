using System.ComponentModel.DataAnnotations;

namespace HomeLibrary.DTO
{
    public class CreateBookRequest
    {
        [Required(ErrorMessage = "Необходимо указать название книги.")]
        [StringLength(300, ErrorMessage = "Название книги не должно превышать 300 символов.")]
        public string Title { get; set; } = null!;

        [Range(1, 9999, ErrorMessage = "Год издания должен быть от 1 до 9999.")]
        public int PublicationYear { get; set; }

        public string? Content { get; set; }

        [MinLength(1, ErrorMessage = "Необходимо выбрать хотя бы одного автора.")]
        public List<int> AuthorIds { get; set; } = [];
    }
}