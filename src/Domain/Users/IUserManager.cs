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
        Task<Client> GetClientAsync(int id);

        /// <summary>
        /// get Administrator by user id
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>user or null if not exist</returns>
        Task<Administrator> GetAdministratorAsync(int id);
        /// <summary>
        /// get client by user id
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>user or null if not exist</returns>
        Task<Client> GetClientAsync(string account);

        /// <summary>
        /// get Administrator by user id
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>user or null if not exist</returns>
        Task<Administrator> GetAdministratorAsync(string account);
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="role">角色，null 为所有角色</param>
        /// <returns></returns>
        Task<Paginator> GetUserListAsync(Paginator pager, User.RoleCategories? role = null);
        /// <summary>
        /// 是否有这个用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns>(是否有这个用户，用户名)</returns>
        Task<bool> HasUserAsync(string account);
        /// <summary>
        /// user login，
        /// 客户或管理员都可以登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns>(登录后的用户，登录失败的原因)</returns>
        Task<(User, string)> LoginAsync(Models.Login model);
    }
}
