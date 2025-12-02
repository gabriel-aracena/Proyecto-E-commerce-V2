
using OnlineSpot.Data.Application.DTO;
using OnlineSpot.Data.Application.ViewModels;
using OnlineSpot.Data.Domain.Entities;

namespace OnlineSpot.Data.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> LoginAsync(LoginViewModel loginVm);
        Task<BasicUserInfoDTO?> GetBasicUserInfoByIdAsync(Guid id);
       


    }
}
