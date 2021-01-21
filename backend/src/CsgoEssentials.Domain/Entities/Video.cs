using System.ComponentModel.DataAnnotations;
using System;
using CsgoEssentials.Domain.Enum;

namespace CsgoEssentials.Domain.Entities
{
    public class Video
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(maximumLength: 60, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres.", MinimumLength = 4)]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public EGrenadeType GrenadeType { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public ETick TickRate { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(maximumLength: 60, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres.", MinimumLength = 3)]

        [MaxLength(length: 60, ErrorMessage = "O campo {0} possui um limite de até {1} caracteres.")]
        public string Description { get; set; }

        public User User { get; set; }
        public Map Map { get; set; }

    }
}
