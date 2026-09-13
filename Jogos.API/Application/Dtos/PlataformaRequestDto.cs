using System.ComponentModel.DataAnnotations;

namespace Jogos.API.Application.Dtos
{
    public class PlataformaRequestDto
    {
        [Required(ErrorMessage = "O nome da plataforma é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome da plataforma deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; }
    }
}
