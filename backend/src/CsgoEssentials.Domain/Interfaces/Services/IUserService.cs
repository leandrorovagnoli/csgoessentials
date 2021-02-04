using CsgoEssentials.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CsgoEssentials.Domain.Interfaces.Services
{
    public interface IUserService : IService<User>
    {
        Task<User> GetByIdWithRelationship(int id);
    }
}
