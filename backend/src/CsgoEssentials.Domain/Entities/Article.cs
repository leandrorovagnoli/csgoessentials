using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CsgoEssentials.Domain.Entities
{
    public class Article
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "O Campo {0} precisa ter entre {2} até {1} Caracteres")]
        public string Name { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set;}

        [Required]
        [StringLength(150, MinimumLength = 5, ErrorMessage = "O Campo {0} precisa ter entre {2} até {1} Caracteres")]
        public string Description { get; set; }


        // Falta Adicionar a propriedade "AUTOR" para fazer referencia.

    }
}
