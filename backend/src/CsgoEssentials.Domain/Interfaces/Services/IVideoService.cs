using CsgoEssentials.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CsgoEssentials.Domain.Interfaces.Services
{
    public interface IVideoService : IService<Video>
    {
        Task<Video> GetByIdWithRelationship(int id);

        Task<IList<Video>> Filter(Query query);
    }
}
