using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Jogos.API.Domain.Entities
{
    [Table("tb_plataforma")]
    [Index(nameof(Nome), IsUnique = true, Name = "IDX_plataforma_nome")]
    public class PlataformaEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da plataforma é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome da plataforma deve ter entre 2 e 100 caracteres")]
        [Column("c_nome")]
        public string Nome { get; set; }

        [JsonIgnore]
        public ICollection<JogoEntity>? Jogos { get; set; }
    }
}
