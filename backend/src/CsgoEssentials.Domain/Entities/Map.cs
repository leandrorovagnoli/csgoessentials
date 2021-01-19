using System.ComponentModel.DataAnnotations;


namespace CsgoEssentials.Domain.Entities
{
    public class Map
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(maximumLength: 60, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres.", MinimumLength = 4)]
        public string MapName { get; set; }

        [StringLength(maximumLength: 60, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres.", MinimumLength = 4)]
        public Enum.GrenadeType GrenadeType { get; set; }

        [StringLength(maximumLength: 3, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres.", MinimumLength = 1)]
        public Enum.Tick TickRate { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(maximumLength: 60, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres.", MinimumLength = 3)]
        
        [MaxLength(length: 60, ErrorMessage = "O campo {0} possui um limite de até {1} caracteres.")]
        public string Description { get; set; }

    }
}
