using Jogos.API.Domain.Entities;

namespace Jogos.API.Application.Dtos
{
    public class JogoResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Preco { get; set; }
        public DateTime DataLancamento { get; set; }
        public int DesenvolvedoraId { get; set; }
        public DesenvolvedoraEntity? Desenvolvedora { get; set; }
        public ICollection<CategoriaEntity>? Categorias { get; set; }
        public ICollection<PlataformaEntity>? Plataformas { get; set; }
    }
}
