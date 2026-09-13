using System.ComponentModel.DataAnnotations;

namespace Jogos.API.Application.Dtos
{
    public class DesenvolvedoraRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nome { get; set; }
    }
}
