using CsgoEssentials.Domain.Enum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CsgoEssentials.Domain.Entities
{
    public class User
    {
        public User(string name, string userName, string password, EUserRole role)
        {
            Name = name;
            UserName = userName;
            Password = password;
            Role = role;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(maximumLength: 60, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres.", MinimumLength = 4)]
        [DisplayName("Nome")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(maximumLength: 60, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres.", MinimumLength = 4)]
        [DisplayName("Nome de Usuário")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(maximumLength: 60, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres.", MinimumLength = 6)]
        [DisplayName("Senha")]
        public string Password { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Função")]
        public EUserRole Role { get; set; }

        public override string ToString()
        {
            return $"Nome: {Name}, Nome de Usuário: {UserName}";
        }
    }
}
