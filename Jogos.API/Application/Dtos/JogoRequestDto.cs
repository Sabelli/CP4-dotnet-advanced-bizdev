using System.ComponentModel.DataAnnotations;

namespace Jogos.API.Application.Dtos
{
    public class JogoRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nome { get; set; }
        public double Preco { get; set; }
        [Required]
        public string Plataforma { get; set; }
        public DateTime DataLancamento { get; set; }
        [Required]
        public int DesenvolvedoraId { get; set; }
    }
}
