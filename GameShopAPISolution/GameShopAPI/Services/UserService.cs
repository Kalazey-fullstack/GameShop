using GameShopAPI.Helpers;
using GameShopAPI.Models.Users;
using GameShopAPI.Repositories.Interfaces;
using GameShopAPI.Services.Interfaces;

namespace GameShopAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User, Guid> _userRepository;
        private readonly Encryptor _encryptor;

        public UserService(IRepository<User, Guid> userRepository)
        {
            _userRepository = userRepository;
            _encryptor = new Encryptor();
        }

        public async Task<User> Create(User user)
        {
            try
            {
                user.Password = _encryptor.EncryptPassword(user.Password);
                return await _userRepository.Add(user);
            }catch (Exception ex)
            {
                Console.WriteLine($"Error adding user {user.Email} : {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAll() => await _userRepository.GetAll();

        public async Task<User?> GetByEmail(string email) => await _userRepository.Get(u => u.Email == email);

        public async Task<User?> GetById(Guid id) => await _userRepository.GetById(id);

        public async Task<User?> Update(Guid id, User user)
        {
            try
            {
                user.Id = id;
                user.Password = _encryptor.EncryptPassword(user.Password);
                return await _userRepository.Update(user) ?? throw new KeyNotFoundException($"The user with id {id} cannot be found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error modifying user with id {id} : {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        public async Task Delete(Guid id)
        {
            try
            {
                if (!await _userRepository.Delete(id))
                    throw new KeyNotFoundException($"The user with id {id} cannot be found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user with id {id} : {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}
