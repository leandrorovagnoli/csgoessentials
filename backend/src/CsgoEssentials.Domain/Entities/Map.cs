using CsgoEssentials.Infra.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CsgoEssentials.Domain.Entities
{
    public class Map : IValidatableObject
    {
        #region Constructor

        public Map(string name, string description)
        {
            Name = name;
            Description = description;
        }

        #endregion 

        #region Properties

        [Key]
        public int Id { get; set; }

        [DisplayName(Messages.MAPA)]
        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [StringLength(maximumLength: 60, ErrorMessage = Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [StringLength(maximumLength: 60, ErrorMessage = Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, MinimumLength = 3)]
        [DisplayName(Messages.DESCRICAO)]
        public string Description { get; set; }

        #endregion

        #region Methods
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // Validating Mapname
            Name = Name.ToLower();
            var regex = "1";
            var match = Regex.Match(Name, regex, RegexOptions.IgnoreCase);

            if (!match.Success)
                results.Add(new ValidationResult(string.Format(Messages.CAMPO_INVALIDO, Messages.MAPA_EXISTENTE), new[] { nameof(Name) }));

            return results;
        }

        #endregion
    }
}
