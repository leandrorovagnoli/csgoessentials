using CsgoEssentials.Infra.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CsgoEssentials.Domain.Entities
{
    public class Article : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [DisplayName(Messages.TITULO)]
        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [StringLength(20, MinimumLength = 2, ErrorMessage = Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES)]
        public string Title { get; set; }

        [DisplayName(Messages.DATA_DE_PUBLICACAO)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [DisplayName(Messages.DESCRICAO)]
        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [StringLength(150, MinimumLength = 5, ErrorMessage = Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES)]
        public string Description { get; set; }

        [DisplayName(Messages.USUARIO)]
        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        public User User { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // Validations

            return results;
        }
    }
}
