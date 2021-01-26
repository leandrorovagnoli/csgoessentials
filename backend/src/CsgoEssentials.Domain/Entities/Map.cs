using CsgoEssentials.Infra.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CsgoEssentials.Domain.Entities
{
    public class Map : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [DisplayName(Messages.NOME)]
        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [StringLength(maximumLength: 60, ErrorMessage = Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, MinimumLength = 4)]
        public string Name { get; set; }

        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [StringLength(maximumLength: 60, ErrorMessage = Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, MinimumLength = 3)]
        [DisplayName(Messages.DESCRICAO)]
        public string Description { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // Validations

            return results;
        }
    }
}
