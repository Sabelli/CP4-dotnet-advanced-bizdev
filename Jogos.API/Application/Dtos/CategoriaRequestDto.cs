using System.ComponentModel.DataAnnotations;

namespace Jogos.API.Application.Dtos
{
    public class CategoriaRequestDto
    {
        [Required(ErrorMessage = "O nome da categoria é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome da categoria deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; }
    }
}
