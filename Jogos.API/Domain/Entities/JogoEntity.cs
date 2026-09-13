using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jogos.API.Domain.Entities
{
    [Table("tb_jogo")]
    [Index(nameof(Nome), nameof(Plataforma), IsUnique = true, Name = "IDX_jogo_nome_plataforma")]
    public class JogoEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do jogo é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do jogo deve ter entre 3 e 100 caracteres")]
        [Column("c_nome")]
        public string Nome { get; set; }

        [Column("c_preco")]
        public double Preco { get; set; }

        [Required(ErrorMessage = "A plataforma do jogo é obrigatória")]
        [Column("c_plataforma")]
        public string Plataforma { get; set; }

        [Column("c_data_lancamento")]
        public DateTime DataLancamento { get; set; }

        [Column("c_desenvolvedora_id")]
        public int DesenvolvedoraId { get; set; }

        public DesenvolvedoraEntity? Desenvolvedora { get; set; }

        public ICollection<CategoriaEntity>? Categorias { get; set; }
    }
}
