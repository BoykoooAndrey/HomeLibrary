using System.ComponentModel.DataAnnotations;

namespace HomeLibrary.DTO
{
    public class CreateAuthorRequest
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = null!;

        [StringLength(100)]
        public string? MiddleName { get; set; }
    }
}