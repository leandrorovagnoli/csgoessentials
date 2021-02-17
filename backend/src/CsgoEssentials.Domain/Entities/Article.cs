using CsgoEssentials.Domain.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CsgoEssentials.Domain.Entities
{
    public class Article : Entity, IValidatableObject
    {
        public Article(string title, DateTime releaseDate, string description, int userId)
        {
            Title = title;
            ReleaseDate = releaseDate;
            Description = description;
            UserId = userId;
        }

        [DisplayName(Messages.TITULO)]
        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [StringLength(200, MinimumLength = 10, ErrorMessage = Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES)]
        public string Title { get; set; }

        [DisplayName(Messages.DATA_DE_PUBLICACAO)]
        [DataType(DataType.Date, ErrorMessage = Messages.DATA_COM_FORMATO_INVALIDO)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [DisplayName(Messages.DESCRICAO)]
        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [StringLength(2000, MinimumLength = 20, ErrorMessage = Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES)]
        public string Description { get; set; }

        [DisplayName(Messages.USUARIO)]
        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        public int UserId { get; set; }

        public User User { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // Validations

            return results;
        }
    }
}
