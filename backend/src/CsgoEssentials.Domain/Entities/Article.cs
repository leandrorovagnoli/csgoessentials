using System;
using System.ComponentModel.DataAnnotations;


namespace CsgoEssentials.Domain.Entities
{
    public class Article
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "O Campo {0} precisa ter entre {2} até {1} Caracteres")]
        public string ArticleName { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set;}

        [Required]
        [StringLength(150, MinimumLength = 5, ErrorMessage = "O Campo {0} precisa ter entre {2} até {1} Caracteres")]
        public string Description { get; set; }

        [Required]
        public User User { get; set; }

    }
}
