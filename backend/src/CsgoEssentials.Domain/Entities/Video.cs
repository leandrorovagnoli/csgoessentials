using CsgoEssentials.Domain.Enum;
using CsgoEssentials.Domain.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CsgoEssentials.Domain.Entities
{
    public class Video : Entity, IValidatableObject
    {
        #region Constructor

        public Video()
        {

        }

        public Video(string title, DateTime releaseDate, EGrenadeType grenadeType, ETick tickRate, string description, string videoURL, int userId, int mapId)
        {
            Title = title;
            ReleaseDate = releaseDate;
            GrenadeType = grenadeType;
            TickRate = tickRate;
            Description = description;
            VideoURL = videoURL;
            UserId = userId;
            MapId = mapId;
        }

        #endregion

        #region Properties

        [DisplayName(Messages.TITULO)]
        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [StringLength(maximumLength: 60, ErrorMessage = Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, MinimumLength = 4)]
        public string Title { get; set; }

        [DisplayName(Messages.DATA_DE_PUBLICACAO)]
        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [DisplayName(Messages.TIPO_DE_GRANADA)]
        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        public EGrenadeType GrenadeType { get; set; }

        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        public ETick TickRate { get; set; }

        [DisplayName(Messages.DESCRICAO)]
        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [StringLength(maximumLength: 120, ErrorMessage = Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, MinimumLength = 20)]
        public string Description { get; set; }

        [DisplayName(Messages.VIDEOURL)]
        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [Url(ErrorMessage = Messages.URL_INVALIDA)]
        [StringLength(maximumLength: 100, ErrorMessage = Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, MinimumLength = 2)]
        public string VideoURL { get; set; }

        [DisplayName(Messages.USUARIO)]
        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        public int UserId { get; set; }

        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [DisplayName(Messages.MAPA)]
        public int MapId { get; set; }

        public User User { get; set; }

        public Map Map { get; set; }

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
