using CsgoEssentials.Domain.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CsgoEssentials.Domain.Entities
{
    public class PlayerAttribute : Entity, IValidatableObject
    {
        #region Constructor

        public PlayerAttribute(string name, string description, float? grade)
        {
            Name = name;
            Description = description;
            Grade = grade;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of the player attribute. Ex: Sound skill
        /// </summary>
        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [StringLength(maximumLength: 60, ErrorMessage = Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, MinimumLength = 2)]
        [DisplayName(Messages.NOME)]
        public string Name { get; set; }

        /// <summary>
        /// Description for the player attribute. Ex: Ability to hear noise...
        /// </summary>
        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [StringLength(maximumLength: 250, ErrorMessage = Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, MinimumLength = 2)]
        [DisplayName(Messages.DESCRICAO)]
        public string Description { get; set; }

        /// <summary>
        /// Grade that determines the level for this skill
        /// </summary>
        [DisplayName(Messages.NOTA)]
        public float? Grade { get; set; }

        #endregion

        #region Methods

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // Validations

            return results;
        }

        #endregion
    }
}
