using CsgoEssentials.Domain.Enum;
using CsgoEssentials.Domain.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CsgoEssentials.Domain.Entities
{
    public class User : Entity, IValidatableObject
    {
        #region Constructor

        public User(string firstName, string lastName, string nickName, EPlayerRole playerRole1, EPlayerRole playerRole2, EPlayerRole playerRole3, string email, string userName, string password, EUserRole userRole)
        {
            FirstName = firstName;
            LastName = lastName;
            NickName = nickName;
            PlayerRole1 = playerRole1;
            PlayerRole2 = playerRole2;
            PlayerRole3 = playerRole3;
            Email = email;
            UserName = userName;
            Password = password;
            UserRole = userRole;
            Articles = new List<Article>();
            Videos = new List<Video>();
        }

        #endregion

        #region Properties

        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [StringLength(maximumLength: 60, ErrorMessage = Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, MinimumLength = 3)]
        [DisplayName(Messages.NOME)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [StringLength(maximumLength: 60, ErrorMessage = Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, MinimumLength = 3)]
        [DisplayName(Messages.SOBRENOME)]
        public string LastName { get; set; }

        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [StringLength(maximumLength: 60, ErrorMessage = Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, MinimumLength = 2)]
        [DisplayName(Messages.APELIDO)]
        public string NickName { get; set; }

        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [DisplayName(Messages.FUNCAO_PRINCIPAL_NO_JOGO)]
        public EPlayerRole PlayerRole1 { get; set; }

        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [DisplayName(Messages.FUNCAO_SECUNDARIA_NO_JOGO)]
        public EPlayerRole PlayerRole2 { get; set; }

        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [DisplayName(Messages.FUNCAO_OPCIONAL_NO_JOGO)]
        public EPlayerRole PlayerRole3 { get; set; }

        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [EmailAddress(ErrorMessage = Messages.CAMPO_INVALIDO)]
        [DisplayName(Messages.EMAIL)]
        public string Email { get; set; }

        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [StringLength(maximumLength: 60, ErrorMessage = Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, MinimumLength = 4)]
        [DisplayName(Messages.NOME_DE_USUARIO)]
        public string UserName { get; set; }

        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [StringLength(maximumLength: 60, ErrorMessage = Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, MinimumLength = 6)]
        [DisplayName(Messages.SENHA)]
        public string Password { get; set; }

        [Required(ErrorMessage = Messages.CAMPO_OBRIGATORIO)]
        [DisplayName(Messages.FUNCAO_DE_USUARIO)]
        public EUserRole UserRole { get; set; }

        [DisplayName(Messages.ARTIGOS)]
        public IList<Article> Articles { get; set; }

        [DisplayName(Messages.VIDEO)]
        public IList<Video> Videos { get; set; }

        #endregion

        #region Methods

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // Validating Username
            UserName = UserName.ToLower();
            var regex = @"^[a-z0-9]+(?:[_-]?[a-z0-9])*$";
            var match = Regex.Match(UserName, regex, RegexOptions.IgnoreCase);

            if (!match.Success)
                results.Add(new ValidationResult(string.Format(Messages.CAMPO_INVALIDO, Messages.NOME_DE_USUARIO), new[] { nameof(UserName) }));

            return results;
        }

        #endregion
    }
}
