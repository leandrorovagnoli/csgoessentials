using CsgoEssentials.Domain.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;

namespace CsgoEssentials.Domain.Entities
{
    public class Entity : IEntity<int>
    {
        [Key]
        public int Id { get; set; }
    }
}
