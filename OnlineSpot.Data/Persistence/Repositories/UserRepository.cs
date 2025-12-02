using Microsoft.EntityFrameworkCore;
using OnlineSpot.Data.Application.DTO;
using OnlineSpot.Data.Application.Helpers;
using OnlineSpot.Data.Application.ViewModels;
using OnlineSpot.Data.Domain.Entities;
using OnlineSpot.Data.Domain.Interfaces;
using OnlineSpot.Data.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSpot.Data.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly OnlineSpotDbContext _dbContext;

        public UserRepository(OnlineSpotDbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<User> LoginAsync(LoginViewModel loginVm)
        {
            string encryptedPassword = PasswordEncryption.ComputeSha256Hash(loginVm.Password);

            return await _dbContext.Set<User>()
            .FirstOrDefaultAsync(user =>
                    user.userName == loginVm.Username && user.passwordHash == encryptedPassword);

        }

        public override async Task<User> AddAsync(User userEntity)
        {
            
            if (userEntity.passwordHash.Length != 64 || !System.Text.RegularExpressions.Regex.IsMatch(userEntity.passwordHash, @"^[a-fA-F0-9]{64}$"))
            {
                userEntity.passwordHash = PasswordEncryption.ComputeSha256Hash(userEntity.passwordHash);
            }

            return await base.AddAsync(userEntity);
        }


        public override async Task UpdateAsync(User userEntity, Guid id)
        {
            // Obtener el usuario existente de la base de datos
            var existingUser = await _dbContext.Set<User>().FindAsync(id);
            if (existingUser == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            // Solo hashear la contraseña si se proporciona una nueva contraseña
            if (!string.IsNullOrEmpty(userEntity.passwordHash) && existingUser.passwordHash != userEntity.passwordHash)
            {
                existingUser.passwordHash = PasswordEncryption.ComputeSha256Hash(userEntity.passwordHash);
            }

            // Actualizar otras propiedades del usuario
            existingUser.IsActive = userEntity.IsActive;
            existingUser.userName = userEntity.userName;
            existingUser.role = userEntity.role;
            existingUser.email = userEntity.email;
            existingUser.name = userEntity.name;
            // Actualiza otras propiedades según sea necesario

            // Guardar los cambios en la base de datos
            await _dbContext.SaveChangesAsync();
        }
        public async Task<BasicUserInfoDTO?> GetBasicUserInfoByIdAsync(Guid id)
        {
            return await _dbContext.Users
                .Where(u => u.id == id && u.IsActive)
                .Select(u => new BasicUserInfoDTO
                {
                    Username = u.userName,
                })
                .FirstOrDefaultAsync();
        }

    }
}
