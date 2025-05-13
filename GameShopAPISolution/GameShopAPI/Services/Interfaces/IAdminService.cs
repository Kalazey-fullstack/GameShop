using GameShopAPI.Models.Users;

namespace GameShopAPI.Services.Interfaces
{
    public interface IAdminUService
    {
        Task<IEnumerable<AdminU>> GetAll();
        Task<AdminU> GetById(Guid id);
        Task<AdminU> GetByEmail(string email);
        Task<AdminU> Create(AdminU adminU);
        Task<AdminU> Update(Guid id, AdminU adminU);
        Task<AdminU> DeleteById(Guid id);
    }
}
