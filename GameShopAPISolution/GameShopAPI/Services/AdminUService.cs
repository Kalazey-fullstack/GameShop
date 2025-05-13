using GameShopAPI.Helpers;
using GameShopAPI.Models.Users;
using GameShopAPI.Repositories.Interfaces;
using GameShopAPI.Services.Interfaces;

namespace GameShopAPI.Services
{
    public class AdminUService : IAdminUService
    {
        private readonly IRepository<AdminU, Guid> _adminRepository;
        private readonly Encryptor _encryptor;

        public AdminUService(IRepository<AdminU, Guid> adminRepository)
        {
            _adminRepository = adminRepository;
            _encryptor = new Encryptor();
        }

        public async Task<AdminU> Create(AdminU adminU)
        {
            try
            {
                adminU.Password = _encryptor.EncryptPassword(adminU.Password);
                return await _adminRepository.Add(adminU);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Add error for administrator {adminU.Email} : {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        public async Task<IEnumerable<AdminU>> GetAll() => await _adminRepository.GetAll();

        public async Task<AdminU> GetByEmail(string email) => await _adminRepository.Get(ad => ad.Email == email);

        public async Task<AdminU> GetById(Guid id) => await _adminRepository.GetById(id);

        public async Task<AdminU> Update(Guid id, AdminU adminU)
        {
            try
            {
                adminU.Id = id;
                adminU.Password = _encryptor.EncryptPassword(adminU.Password!);
                return await _adminRepository.Update(adminU) ?? throw new KeyNotFoundException($"Admin with id {id} not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Modification error for administrator with id {id} : {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        public async Task DeleteById(Guid id)
        {
            try
            {
                if (!await _adminRepository.Delete(id))
                    throw new KeyNotFoundException($"Administrator with id {id} not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Remove error for administrator with id {id} : {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}
