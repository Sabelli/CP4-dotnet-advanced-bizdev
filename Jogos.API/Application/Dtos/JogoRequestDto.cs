using System.ComponentModel.DataAnnotations;

namespace Jogos.API.Application.Dtos
{
    public class JogoRequestDto
    {
        [Required(ErrorMessage = "O nome do jogo é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do jogo deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; }
        public double Preco { get; set; }
        public DateTime DataLancamento { get; set; }
        [Required(ErrorMessage = "A desenvolvedora do jogo é obrigatória")]
        public int DesenvolvedoraId { get; set; }
        public IEnumerable<int>? CategoriaIds { get; set; }
        public IEnumerable<int>? PlataformaIds { get; set; }
    }
}
