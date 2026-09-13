using Jogos.API.Domain.Entities;

namespace Jogos.API.Domain.Interfaces
{
    public interface IDesenvolvedoraRepository
    {
        Task<IEnumerable<DesenvolvedoraEntity>> ObterTodosAsync(int Deslocamento, int RegistroRetornado);
        DesenvolvedoraEntity? ObterUm(int Id);
        DesenvolvedoraEntity? Adicionar(DesenvolvedoraEntity entity);
        DesenvolvedoraEntity? Editar(int Id, DesenvolvedoraEntity entity);
        DesenvolvedoraEntity? Deletar(int Id);
    }
}
