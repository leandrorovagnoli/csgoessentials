using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Repository;
using CsgoEssentials.Domain.Interfaces.Services;
using System;
using CsgoEssentials.Domain.Utils;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;

namespace CsgoEssentials.Domain.Services
{
    public class PlayerAssessmentService : IPlayerAssessmentService
    {
        private readonly IPlayerAssessmentRepository _playerAssessmentRepository;

        public PlayerAssessmentService(IPlayerAssessmentRepository PlayerAssessmentRepository)
        {
            _playerAssessmentRepository = PlayerAssessmentRepository;
        }

        public async Task<PlayerAssessment> Add(PlayerAssessment entity)
        {
            return await _playerAssessmentRepository.Add(entity);
        }

        public async Task Delete(PlayerAssessment entity)
        {
            await _playerAssessmentRepository.Delete(entity);
        }

        public async Task<IEnumerable<PlayerAssessment>> Find(Expression<Func<PlayerAssessment, bool>> predicate)
        {
            return await _playerAssessmentRepository.Find(predicate);
        }

        public async Task<IEnumerable<PlayerAssessment>> GetAll()
        {
            return await _playerAssessmentRepository.GetAll();
        }

        public async Task<PlayerAssessment> GetById(int id)
        {
            return await _playerAssessmentRepository.GetById(id);
        }      

        public async Task Update(PlayerAssessment entity)
        {
            await _playerAssessmentRepository.Update(entity);
        }
    }
}












