using System.ComponentModel.DataAnnotations;

namespace Jogos.API.Application.Dtos
{
    public class CategoriaRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nome { get; set; }
    }
}
