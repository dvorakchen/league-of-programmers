using System.Threading.Tasks;

namespace Domain.Users
{
    /// <summary>
    /// the user manager usage DI，
    /// 该接口用户依赖注入
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// get client by user id
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>user or null if not exist</returns>
        Task<Client> GetClient(int id);

        /// <summary>
        /// get Administrator by user id
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>user or null if not exist</returns>
        Task<Administrator> GetAdministrator(int id);
        /// <summary>
        /// 是否有这个用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns>(是否有这个用户，用户名)</returns>
        Task<(bool, string)> HasUser(int id);
    }
}
