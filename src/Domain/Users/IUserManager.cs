using System.Threading.Tasks;

namespace Domain.Users
{
    public interface IUserManager
    {
        /// <summary>
        /// get user by user id
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>user or null if not exist</returns>
        Task<User> GetUser(int id);
    }
}
