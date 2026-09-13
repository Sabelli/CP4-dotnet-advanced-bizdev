using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jogos.API.Domain.Entities
{
    [Table("tb_categoria")]
    [Index(nameof(Nome), IsUnique = true, Name = "IDX_categoria_nome")]
    public class CategoriaEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da categoria � obrigat�rio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome da categoria deve ter entre 3 e 100 caracteres")]
        [Column("c_nome")]
        public string Nome { get; set; }

        public ICollection<JogoEntity>? Jogos { get; set; }
    }
}
