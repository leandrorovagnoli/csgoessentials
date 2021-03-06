﻿using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Repository;
using CsgoEssentials.Domain.Interfaces.Services;
using CsgoEssentials.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CsgoEssentials.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Add(User entity)
        {
            await CheckUserNameDuplicity(entity);
            await EncryptPassword(entity);

            return await _userRepository.Add(entity);
        }

        public async Task Delete(User entity)
        {
            await CheckUserHasRelationship(entity);
            await _userRepository.Delete(entity);
        }

        public async Task<IEnumerable<User>> Find(Expression<Func<User, bool>> predicate)
        {
            return await _userRepository.Find(predicate);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<User> GetById(int id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<User> GetByIdWithRelationship(int id)
        {
            return await _userRepository.GetByIdWithRelationship(id);
        }

        public async Task Update(User entity)
        {
            await EncryptPassword(entity);
            await CheckUserNameDuplicity(entity);
            await PreventUsernameChange(entity);

            await _userRepository.Update(entity);
        }

        private async Task EncryptPassword(User entity)
        {
            if (entity.Id == 0)
            {
                entity.Password = MD5Hash.CalculaHash(entity.Password);
                return;
            }

            //updating user
            var user = await GetById(entity.Id);
            if (user != null && user.Password != entity.Password)
                entity.Password = MD5Hash.CalculaHash(entity.Password);
        }

        private async Task CheckUserNameDuplicity(User entity)
        {
            var users = await Find(x => x.UserName == entity.UserName);
            var user = users.FirstOrDefault();

            if (user != null && user.Id != entity.Id)
                throw new InvalidOperationException(Messages.NOME_DE_USUARIO_JA_EXISTENTE);
        }

        private async Task PreventUsernameChange(User entity)
        {
            if (entity.Id == 0)
                return;

            var users = await Find(x => x.Id == entity.Id);
            var user = users.FirstOrDefault();

            if (user != null && user.UserName != entity.UserName)
                throw new InvalidOperationException(Messages.NAO_E_PERMITIDO_ALTERAR_NOME_DE_USUARIO);
        }

        private async Task CheckUserHasRelationship(User entity)
        {
            var user = await GetByIdWithRelationship(entity.Id);

            if (user != null && (user.Articles.Any() || user.Videos.Any()))
                throw new InvalidOperationException(Messages.NAO_FOI_POSSIVEL_REMOVER_USUARIO_POSSUI_ARTIGOS_OU_VIDEOS_CADASTRADOS);
        }
    }
}
