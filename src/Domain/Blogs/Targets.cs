using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB;
using Microsoft.EntityFrameworkCore;

namespace Domain.Blogs
{
    /// <summary>
    /// 标签
    /// </summary>
    internal class Targets
    {
        /// <summary>
        /// 添加标签，返回标签 ID 列表
        /// </summary>
        /// <param name="newTargets"></param>
        /// <returns></returns>
        public static async Task<int[]> AppendTargets(string[] newTargets)
        {
            await using var db = new LOPDbContext();

            List<int> ids = new List<int>(newTargets.Length);

            foreach (string target in newTargets)
            {
                var model = await db.Targets.AsNoTracking().FirstOrDefaultAsync(t => t.Name.Equals(target, StringComparison.OrdinalIgnoreCase));
                if (model is null)
                {
                    model = new DB.Tables.Target
                    { 
                        Name = target
                    };
                    db.Targets.Add(model);
                    await db.SaveChangesAsync();
                }
                ids.Add(model.Id);
            }
            return ids.ToArray();
        }
    }
}
