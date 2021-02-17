using CsgoEssentials.Domain.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CsgoEssentials.Domain.Entities
{
    public class Map : Entity, IValidatableObject
    {
        #region Constructor

        public Map(string name, string description)
        {
            Name = name;
            Description = description;
            Videos = new List<Video>();
        }

        #endregion 

        #region Properties

        [DisplayName(Messages.MAPA)]
        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [StringLength(maximumLength: 60, ErrorMessage = Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [StringLength(maximumLength: 60, ErrorMessage = Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, MinimumLength = 3)]
        [DisplayName(Messages.DESCRICAO)]
        public string Description { get; set; }

        [DisplayName(Messages.VIDEO)]
        public IList<Video> Videos { get; set; }

        #endregion

        #region Methods
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // Validating Mapname
            
            return results;
        }

        #endregion
    }
}
