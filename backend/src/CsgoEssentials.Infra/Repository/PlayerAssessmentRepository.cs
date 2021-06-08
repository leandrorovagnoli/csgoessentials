using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Repository;
using CsgoEssentials.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CsgoEssentials.Infra.Repository
{
    public class PlayerAssessmentRepository : EFRepository<PlayerAssessment>, IPlayerAssessmentRepository
    {
        public PlayerAssessmentRepository(DataContext context) : base(context)
        {

        }        
    }
}
