using CsgoEssentials.Domain.Enum;

namespace CsgoEssentials.Domain.Entities
{
    public class Query
    {
        public int? MapId { get; set; }

        public ETick? TickRate { get; set; }

        public EGrenadeType? GrenadeType { get; set; }

        public int? UserId { get; set; }
    }
}
